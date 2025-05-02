from django import forms
from django.utils import timezone

from .models import DriveLog, JobSite


class StartDriveForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(StartDriveForm, self).__init__(*args, **kwargs)
        self.fields["date"].initial = timezone.now().date()
        self.fields["job_site"].initial = DriveLog.get_last_logged_job_site()

    class Meta:
        model = DriveLog
        fields = ["date", "start_km", "job_site"]
        widgets = {"date": forms.DateInput(attrs={"type": "date"})}


class EndDriveForm(forms.ModelForm):
    class Meta:
        model = DriveLog
        fields = ["end_km", "status"]

    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.initial["status"] = DriveLog.DriveStatus.COMPLETED
        self.fields["status"].widget = forms.HiddenInput()


class JobSiteForm(forms.ModelForm):
    class Meta:
        model = JobSite
        fields = ["name", "address"]
