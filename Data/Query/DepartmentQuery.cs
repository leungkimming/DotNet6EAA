using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Common;

namespace Data {
    public partial class DepartmentQuery : IDepartmentQuery {
        private string _connectionString = string.Empty;

        public DepartmentQuery(IConfiguration conf) {
            _connectionString = conf.GetConnectionString("DDDConnectionString");
        }

        public async Task<IEnumerable<Dep>> GetDepartmentsAsync() {
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();

                IEnumerable<Dep> result = await connection.QueryAsync<Dep>(
                    @"select p.[Name] AS DepartmentName FROM [dbo].[Departments] p");

                return result;
            }
        }
    }
}
