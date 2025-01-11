from django.contrib.auth.models import User
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
