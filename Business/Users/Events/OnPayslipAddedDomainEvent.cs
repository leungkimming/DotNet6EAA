
namespace Business {
    public class OnPayslipIssuedDomainEvent : BaseDomainEvent {
        public Payslip Payslip { get; set; }
    }
}