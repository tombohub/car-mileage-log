using CarMileageLog.Data.Models;
using CarMileageLog.FormDataModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CarMileageLog.ViewModels
{
    public record HomeIndexViewModel
    {
        public required StartDriveFormData StartDriveFormData { get; init; }
        public required EndDriveFormData EndDriveFormData { get; init; }
        public required IEnumerable<SelectListItem>? JobSites { get; init; }
        public required bool IsDriveInProgress { get; init; }
        public DriveLog? DriveInProgress { get; init; }
        public required DateOnly Today { get; init; }
    }
}
