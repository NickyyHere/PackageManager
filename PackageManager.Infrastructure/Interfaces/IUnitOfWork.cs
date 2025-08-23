using FluentResults;

namespace PackageManager.Infrastructure.Interfaces;

public interface IUnitOfWork
{
    Task<Result<IDisposable>> BeginTransactionAsync();
    Task<Result> CommitTransactionAsync();
    Task<Result> RollbackTransactionAsync();
    Task<Result> SaveChangesAsync();
}