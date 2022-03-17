using Business.Base;
using Business.Departments;
using Business.Users.Events;
using Common.Shared;

namespace Business.Users {
    public partial class User : IAggregateRoot {
        public User(string userName
            , string firstName
            , string lastName
            , string address
            , DateTime? birthDate
            , int departmentId
            , float CoefficientsSalary)
        {
            UserName = userName;

            this.Update(
                firstName
                , lastName
                , address
                , birthDate
                , departmentId
                , CoefficientsSalary
            );
        }

        public void Update(string firstName
            , string lastName
            , string address
            , DateTime? birthDate
            , int departmentId
            , float coefficientsSalary)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            BirthDate = birthDate;
            DepartmentId = departmentId;
            CoefficientsSalary = coefficientsSalary;
        }

        public void AddDepartment(Department department) {
            Department = department;
            //DepartmentId = departmentId;
        }

        public Payslip AddPayslip(DateTime date
            , float workingDays
            , decimal bonus
            , bool isPaid
            ) {
            // Make sure there's only one payslip  per month
            var exist = PaySlips.Any(_ => _.Date.Month == date.Month && _.Date.Year == date.Year);
            if (exist)
                throw new PayslipMonthAlreadyExistException(date.Month);

            var payslip = new Payslip(this.Id, date, workingDays, bonus);
            if (isPaid) {
                payslip.Pay(this.CoefficientsSalary);
            }

            PaySlips.Add(payslip);

            if (isPaid && Address != null && payslip.TotalSalary > 0)
            {
                var addEvent = new OnPayslipAddedDomainEvent()
                {
                    Payslip = payslip
                };
                AddEvent(addEvent);

            }

            return payslip;
        }

        public void SendPayslipLetter(Payslip payslip, string letter)
        {
            Payslip? ps = PaySlips.FirstOrDefault(_ => _.Date == payslip.Date);
            if (ps!=null)
            {
                ps.UpdateLetterSent(letter);
            }
        }
    }
}