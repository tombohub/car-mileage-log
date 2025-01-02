namespace CarMileageLog.Data.Models
{
    public class JobSite
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public required string Address { get; set; }
    }
}
