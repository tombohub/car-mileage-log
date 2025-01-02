using System.ComponentModel.DataAnnotations;

namespace CarMileageLog.FormDataModels
{
    public record StartDriveFormData
    {
        [DataType(DataType.Date)]
        public DateOnly Date { get; init; }
        public int StartKilometers { get; init; }
        public int JobSiteId { get; init; }
    }
}
