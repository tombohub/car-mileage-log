from django.urls import path

from . import views
from .views import (
    JobSiteCreateView,
    JobSiteDeleteView,
    JobSiteListView,
    end_drive,
    home,
    start_drive,
)

urlpatterns = [
    path("", home, name="home"),
    path("start-drive", start_drive, name="start-drive"),
    path("end-drive", end_drive, name="end-drive"),
    path("job-sites", JobSiteListView.as_view(), name="jobsite_list"),
    path("job-sites/create", JobSiteCreateView.as_view(), name="job-sites-create"),
    path(
        "job-sites/delete/<int:pk>",
        JobSiteDeleteView.as_view(),
        name="jobsite_confirm_delete",
    ),
    path(
        "job-sites/edit/<int:pk>",
        views.JobSiteUpdateView.as_view(),
        name="jobsite_edit",
    ),
]
