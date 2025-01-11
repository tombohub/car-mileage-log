from django.contrib import messages
from django.contrib.messages.views import SuccessMessageMixin
from django.http import HttpRequest
from django.shortcuts import redirect, render
from django.urls import reverse_lazy
from django.views.generic.edit import CreateView, DeleteView, UpdateView
from django.views.generic.list import ListView

from .forms import EndDriveForm, JobSiteForm, StartDriveForm
from .models import DriveLog, JobSite


# Create your views here.
def home(request: HttpRequest):
    latest_log = DriveLog.get_latest_log()
    if DriveLog.is_drive_in_progress():
        drive_in_progress = DriveLog.get_drive_in_progress()
        if request.method == "POST":
            end_drive_form = EndDriveForm(request.POST)
            if end_drive_form.is_valid():
                if drive_in_progress:
                    drive_in_progress.end_km = end_drive_form.cleaned_data["end_km"]
                    drive_in_progress.status = DriveLog.DriveStatus.COMPLETED
                    drive_in_progress.save()
                    messages.success(request, "Drive ended.")
                return redirect("home")
        else:
            end_drive_form = EndDriveForm()
            context = {
                "form": end_drive_form,
                "is_drive_in_progress": True,
                "drive_log": drive_in_progress,
                "latest_log": latest_log,
            }
    else:
        if request.method == "POST":
            start_drive_form = StartDriveForm(request.POST)
            if start_drive_form.is_valid():
                start_drive_form.save()
                messages.success(request, "Drive started.")
                return redirect("home")
        else:
            start_drive_form = StartDriveForm()
        context = {
            "form": start_drive_form,
            "drive_in_progress": False,
            "latest_log": latest_log,
        }
    return render(request, "drive_log/home.html", context)


class JobSiteListView(ListView):
    model = JobSite
    context_object_name = "job_sites"


class JobSiteCreateView(SuccessMessageMixin, CreateView):
    model = JobSite
    fields = "__all__"
    success_url = reverse_lazy("jobsite_list")
    success_message = "Job site created."


class JobSiteDeleteView(SuccessMessageMixin, DeleteView):
    model = JobSite
    success_url = reverse_lazy("jobsite_list")
    success_message = "Job site deleted"


class JobSiteUpdateView(SuccessMessageMixin, UpdateView):
    model = JobSite
    fields = "__all__"
    success_url = reverse_lazy("jobsite_list")
    success_message = "Job site updated"
