using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Business.Users.Events;
using Business.Users;
using Data.EF.Interfaces;

namespace Service.DomainEventHandlers
{
    public class OnPayslipAddedDomainEventHandler
        : INotificationHandler<OnPayslipAddedDomainEvent>
    {
        public IUnitOfWork _unitOfWork;
        public OnPayslipAddedDomainEventHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task Handle(OnPayslipAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            string letter = "To: "
                + notification.Payslip.User.Address + "\n"
                + "Dear " + notification.Payslip.User.UserName + "\n"
                + "Your Salary, amount to "
                + notification.Payslip.TotalSalary.ToString()
                + ", was debited to your bank on "
                + notification.Payslip.PaymentDate.ToString() + ".\n";
            //await sendLetterService(letter);

            var repository = _unitOfWork.UserRepository();
            var user = await repository.GetAsync(_ => _.Id == notification.Payslip.User.Id);
            if (user != null)
            {
                user.SendPayslipLetter(notification.Payslip, letter);

                await repository.UpdateAsync(user);
            }
        }
    }
}
