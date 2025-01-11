from django.urls import path

from . import views
from .views import JobSiteCreateView, JobSiteDeleteView, JobSiteListView, home

urlpatterns = [
    path("", home, name="home"),
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
