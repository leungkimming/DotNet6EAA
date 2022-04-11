using Business;

namespace Data {
    public class DepartmentRepository : RepositoryBase<Department>
        , IDepartmentRepository {
        public DepartmentRepository(EFContext dbContext) : base(dbContext) {
        }
    }
}