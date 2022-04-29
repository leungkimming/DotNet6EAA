//// Comment all lines to enable NServiceBus Demo

using MediatR;
using Business;
using Data;

namespace Service {
    public class OnPayslipAddedDomainEventHandler
        : INotificationHandler<OnPayslipIssuedDomainEvent> {
        public IUnitOfWork _unitOfWork;
        public OnPayslipAddedDomainEventHandler(IUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
        }
        public async Task Handle(OnPayslipIssuedDomainEvent notification, CancellationToken cancellationToken) {
            string letter = $"To: {notification.Payslip.User.Address} \n"
               + $"Dear {notification.Payslip.User.UserName} \n"
               + $"Your Salary, amount to {notification.Payslip.TotalSalary} "
               + $" was debited to your bank on { notification.Payslip.PaymentDate }.\n";

            var repository = _unitOfWork.UserRepository();
            var user = await repository.GetAsync(_ => _.Id == notification.Payslip.User.Id);
            if (user != null) {
                user.SendPayslipLetter(notification.Payslip.Date, letter);

                await repository.UpdateAsync(user);
            }
        }
    }
}