from django.contrib import admin

from .models import DriveLog, JobSite

# Register your models here.

admin.site.register(JobSite)


class DriveLogAdmin(admin.ModelAdmin):
    list_display = ["date", "job_site"]
    list_filter = ["date", "job_site"]


admin.site.register(DriveLog, DriveLogAdmin)
