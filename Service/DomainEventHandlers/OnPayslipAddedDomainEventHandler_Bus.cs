//// Uncomment all lines to enable NServiceBus Demo

//using MediatR;
//using Business;
//using Data;
//using Messages;
//using NServiceBus;
//using Microsoft.Extensions.Logging;

//namespace Service {
//    public class OnPayslipAddedDomainEventHandler
//        : INotificationHandler<OnPayslipAddedDomainEvent> {
//        public IUnitOfWork _unitOfWork;
//        private readonly ILogger<OnPayslipAddedDomainEventHandler> _log;
//        private readonly IMessageSession _messageSession;
//        public OnPayslipAddedDomainEventHandler(IUnitOfWork unitOfWork,
//            IMessageSession messageSession,
//            ILogger<OnPayslipAddedDomainEventHandler> logger) {
//            this._unitOfWork = unitOfWork;
//            _messageSession = messageSession;
//            _log = logger;
//        }
//        public async Task Handle(OnPayslipAddedDomainEvent notification, CancellationToken cancellationToken) {
//            var Event = new PayslipIssued {
//                UserId = notification.Payslip.UserId,
//                PayslipDate = notification.Payslip.Date,
//                Amount = notification.Payslip.TotalSalary,
//                letter = $"To: {notification.Payslip.User.Address} \n"
//                + $"Dear {notification.Payslip.User.UserName} \n"
//                + $"Your Salary, amount to {notification.Payslip.TotalSalary} "
//                + $" was debited to your bank on { notification.Payslip.PaymentDate }.\n"
//            };

//            // Send the command
//            await _messageSession.Publish(Event);

//            _log.LogInformation($"Payslip Issued, UserId = {Event.UserId}, " +
//                $"Date {Event.PayslipDate}, " +
//                $"Amount {Event.Amount} to Finance and Bank");
//        }
//    }
//}