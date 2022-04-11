
namespace Business {
    public class OnPayslipAddedDomainEvent : BaseDomainEvent {
        public Payslip Payslip { get; set; }
    }
}