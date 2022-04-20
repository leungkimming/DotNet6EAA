using Business;

namespace Data {
    public interface IUnitOfWork {
        Task<int> SaveChangesAsync();

        IAsyncRepository<T> AsyncRepository<T>() where T : RootEntity;

        IUserRepository UserRepository();
        ISystemParametersRepository SystemParametersRepository();
    }
}