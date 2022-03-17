using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.Users
{
    public class AddPayslipRequest
    {
        [Required(ErrorMessage = "Date is mandatory")]
        public DateTime? Date { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Working days is mandatory")]
        [Range(1, 31, ErrorMessage = "Working days must be between 1 to 31")]
        public float? WorkingDays { get; set; }

        [Range(0.0, 10000.00, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Bonus { get; set; }

        [Required(ErrorMessage = "Is paid flag is mandatory")]
        public bool? IsPaid { get; set; }
    }
}