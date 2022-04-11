namespace Common {
    public class AddPayslipResponse : DTObaseResponse {
        public int UserId { get; set; }

        public decimal TotalSalary { get; set; }
        public DateTime? LetterSentDate { get; set; }
        public string? Letter { get; set; }
    }
}