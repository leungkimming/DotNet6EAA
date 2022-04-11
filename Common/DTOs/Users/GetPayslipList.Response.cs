
namespace Common {
    public class PayslipDTO : DTObase {
        public DateTime Date { get; set; }
        public float WorkingDays { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int UserId { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal Bonus { get; set; }
        public DateTime? LetterSentDate { get; set; }
        public string? Letter { get; set; }
    }
}