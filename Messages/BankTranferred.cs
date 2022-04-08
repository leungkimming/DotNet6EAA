using NServiceBus;

namespace Messages
{
    public class BankTranferred : ICommand
    {
        public int UserId { get; set; }
        public DateTime PayslipDate { get; set; }
        public decimal Amount { get; set; }

    }
}