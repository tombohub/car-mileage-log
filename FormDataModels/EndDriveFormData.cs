namespace CarMileageLog.FormDataModels
{
    public record EndDriveFormData
    {
        public int DriveLogId { get; init; }
        public int EndKilometers { get; init; }
    }
}
