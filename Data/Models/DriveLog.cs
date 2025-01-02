using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarMileageLog.Data.Models
{
    public class DriveLog
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }

        [Display(Name = "Start Km")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int StartKilometers { get; set; }

        [Display(Name = "End Km")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? EndKilometers { get; set; }

        public int JobSiteId { get; set; }

        [Display(Name = "Job Site")]
        public JobSite? JobSite { get; set; }

        [Column(TypeName = "varchar(20)")]
        public required DriveStatus Status { get; set; }

    }
}
