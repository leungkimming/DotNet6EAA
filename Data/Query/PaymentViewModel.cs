using Business;
namespace Data {
    public record PaymentSummary {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public List<Payment> Payments { get; set; }
    }

    public record Payment {
        public DateTime PaymentDate { get; set; }
        public decimal TotalSalary { get; set; }
        public float WorkingDays { get; set; }
    }
}
