using Business.Base;
using Data.EF.Interfaces;
using Data.EF.Repositories;
using MediatR;

namespace Data.EF;

public class UnitOfWork : IUnitOfWork
{
    private readonly EFContext _dbContext;
    private readonly IMediator _mediator;
    public UnitOfWork(EFContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        this._mediator = mediator;
    }

    public IAsyncRepository<T> AsyncRepository<T>() where T : RootEntity
    {
        return new RepositoryBase<T>(_dbContext);
    }

    public async Task<int> SaveChangesAsync()
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediator.DispatchDomainEventsAsync(_dbContext);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        return await _dbContext.SaveChangesAsync();
    }

    public IUserRepository UserRepository()
    {
        return new UserRepository(_dbContext);
    }
}
