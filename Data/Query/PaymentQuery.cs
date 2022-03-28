using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Data.Query
{
    public partial class PaymentQuery : IPaymentQuery
    {
        private string _connectionString = string.Empty;

        public PaymentQuery(IConfiguration conf)
        {
            _connectionString = conf.GetConnectionString("DDDConnectionString");
        }
        public async Task<IEnumerable<PaymentSummary>> GetPaymentOfFrequentWorkersAsync(int days)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    @"select p.[Date], p.[TotalSalary], p.[WorkingDays],
                    u.[UserName], u.[FirstName], u.[LastName], d.[Description]
                    FROM [dbo].[PaySlips] p
                    LEFT JOIN [dbo].[Users] u ON u.Id = p.UserId
                    LEFT JOIN [dbo].[Departments] d ON u.DepartmentId1 = d.Id
                    WHERE p.WorkingDays > @days
                    ORDER BY u.[UserName], p.[Date]", new { days }
                    );

                return MapOrderItems(result);
            }
        }
        private List<PaymentSummary> MapOrderItems(dynamic result)
        {
            List<PaymentSummary> Summary = new List<PaymentSummary>();
            string currentUser = null;
            PaymentSummary user = null;

            foreach (dynamic item in result)
            {
                if (currentUser != item.UserName)
                {
                    if (currentUser != null)
                    {
                        Summary.Add(user);
                    }
                    currentUser = item.UserName;
                    user = new PaymentSummary
                    {
                        UserName = item.UserName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Payments = new List<Payment>()
                    };
                    user.Payments.Add(new Payment
                    {
                        PaymentDate = item.Date,
                        TotalSalary = item.TotalSalary,
                        WorkingDays = item.WorkingDays
                    });
                } else {
                    user.Payments.Add(new Payment
                    {
                        PaymentDate = item.Date,
                        TotalSalary = item.TotalSalary,
                        WorkingDays = item.WorkingDays
                    });
                }
            }
            if (currentUser != null)
            {
                Summary.Add(user);
            }

            return Summary;
        }
    }
}
