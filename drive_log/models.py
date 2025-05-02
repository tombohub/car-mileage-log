from django.core.exceptions import ValidationError
from django.db import models


class BaseModel(models.Model):
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    class Meta:
        abstract = True


class JobSite(BaseModel):
    name = models.CharField(max_length=255)
    address = models.CharField(max_length=255)

    def __str__(self) -> str:
        return self.address


class DriveLog(BaseModel):
    class DriveStatus(models.TextChoices):
        IN_PROGRESS = "in_progress"
        COMPLETED = "completed"

    date = models.DateField()
    start_km = models.IntegerField()
    end_km = models.IntegerField(null=True, blank=True)
    status = models.CharField(
        max_length=20, choices=DriveStatus, default=DriveStatus.IN_PROGRESS
    )
    job_site = models.ForeignKey(JobSite, on_delete=models.PROTECT)

    def __str__(self) -> str:
        return str(self.date)

    def _validate_start_km_not_lower_than_previous_end_km(self):
        """
        Validate that start_km is not less than the end_km of the previous drive.

        It doesn't make sense that new drive starts with odometer lower then the end of previous drive. That's impossible.
        """
        previous_log = DriveLog.get_latest_log()
        if previous_log and previous_log.end_km and self.start_km < previous_log.end_km:
            return f"Start kilometers ({self.start_km}) cannot be less than the end kilometers of the previous drive ({previous_log.end_km})."

    def _validate_end_km_is_not_lower_than_start_km(self):
        """
        Validate that end_km is not less than start_km.

        It doesn't make sense for the odometer reading at the end of the drive to be lower than the reading at the start.
        """
        if self.end_km is not None and self.end_km < self.start_km:
            return f"End kilometers ({self.end_km}) cannot be less than start kilometers ({self.start_km})."

    def clean(self) -> None:
        super().clean()

        errors: dict = {}

        start_km_error = self._validate_start_km_not_lower_than_previous_end_km()
        if start_km_error:
            errors["start_km"] = start_km_error

        end_km_error = self._validate_end_km_is_not_lower_than_start_km()
        if end_km_error:
            errors["end_km"] = end_km_error

        raise ValidationError(errors)

    @classmethod
    def get_last_logged_job_site(cls):
        """Get the job site from the last drive log."""
        last_log = cls.objects.order_by("-id").first()
        return last_log.job_site if last_log else None

    @classmethod
    def is_drive_in_progress(cls):
        """Check if there is a drive in progress."""
        return cls.objects.filter(status=cls.DriveStatus.IN_PROGRESS).exists()

    @classmethod
    def get_drive_in_progress(cls):
        """Get the drive log that is in progress."""
        return cls.objects.filter(status=cls.DriveStatus.IN_PROGRESS).first()

    @classmethod
    def get_latest_log(cls):
        """Get the latest drive log based on the id field."""
        return cls.objects.order_by("id").last()
