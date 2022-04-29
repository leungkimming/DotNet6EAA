using Common;

namespace Data {
    public interface IDepartmentQuery {
        Task<IEnumerable<Dep>> GetDepartmentsAsync();
    }
}
