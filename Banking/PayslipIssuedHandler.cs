using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;

namespace Banking
{

    public class PayslipIssuedHandler : IHandleMessages<PayslipIssued>
    {
        static readonly ILog log = LogManager.GetLogger<PayslipIssuedHandler>();

        public Task Handle(PayslipIssued message, IMessageHandlerContext context)
        {
            log.Info($"Received PayslipIssued, UserId = {message.UserId}, " +
                $"Date {message.PayslipDate}, " +
                $"Amount {message.Amount}. " +
                $"Press Enter if bank transfer completed...");
            Console.ReadLine();
    
            var bankTranferred = new BankTranferred
            {
                UserId = message.UserId,
                PayslipDate = message.PayslipDate,
                Amount = message.Amount
            };
            return context.Send("Finance", bankTranferred);
        }
    }
}