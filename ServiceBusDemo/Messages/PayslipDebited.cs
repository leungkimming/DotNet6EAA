using NServiceBus;

namespace Messages
{
    public class PayslipDebited : ICommand
    {
        public int UserId { get; set; }
        public DateTime PayslipDate { get; set; }
        public decimal Amount { get; set; }
        public string letter { get; set; }
    }
}