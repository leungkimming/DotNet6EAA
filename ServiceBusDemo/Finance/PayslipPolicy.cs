using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Messages;

namespace Finance
{
    public class PaySlipPolicy : Saga<PayslipPolicyData>,
        IAmStartedByMessages<BankTranferred>,
        IAmStartedByMessages<PayslipIssued>
    {
        static ILog log = LogManager.GetLogger<PaySlipPolicy>();
        public Task Handle(PayslipIssued message, IMessageHandlerContext context)
        {
            log.Info($"Received Payslip Issue, UserId = {message.UserId}, " +
                $"Date {message.PayslipDate}, " +
                $"Amount {message.Amount}. ");
            Data.IsPayslipIssued = true;
            Data.UserId = message.UserId;
            Data.PayslipDate = message.PayslipDate;
            Data.Amount = message.Amount;
            Data.letter = message.letter;
            return ProcessOrder(context);
        }
        public Task Handle(BankTranferred message, IMessageHandlerContext context)
        {
            log.Info($"Received Bank Transferred, UserId = {message.UserId}, " +
                $"Date {message.PayslipDate}, " +
                $"Amount {message.Amount}. ");
            Data.IsPayslipDebited = true;
            return ProcessOrder(context);
        }
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PayslipPolicyData> mapper)
        {
            mapper.MapSaga(sagaData => sagaData.UserPayDate)
                .ToMessage<BankTranferred>(message => $"{message.UserId}_{message.PayslipDate}")
                .ToMessage<PayslipIssued>(message => $"{message.UserId}_{message.PayslipDate}");
        }

        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsPayslipIssued && Data.IsPayslipDebited)
            {
                await context.Send("API", new PayslipDebited()
                {
                    UserId = Data.UserId,
                    PayslipDate = Data.PayslipDate,
                    Amount = Data.Amount,
                    letter = Data.letter
                });
                MarkAsComplete();
            }
        }
    }
}
