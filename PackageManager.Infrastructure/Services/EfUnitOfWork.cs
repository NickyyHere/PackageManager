using System.Data;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using PackageManager.Core.Interfaces;
using PackageManager.Infrastructure.Data;
namespace PackageManager.Infrastructure.Services;

public class EfUnitOfWork(
    AppDbContext context
) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    private IDbContextTransaction? _transaction;
    public async Task<Result<IDisposable>> BeginTransactionAsync()
    {
        try
        {
            _transaction ??= await _context.Database.BeginTransactionAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to start transaction").CausedBy(ex));
        }
        return Result.Ok((IDisposable)_transaction);
    }

    public async Task<Result> CommitTransactionAsync()
    {
        if (_transaction == null)
        {
            return Result.Fail("No transaction to commit");
        }
        try
        {
            await _transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to commit transaction").CausedBy(ex));
        }
        await _transaction.DisposeAsync();
        _transaction = null;
        return Result.Ok();
    }

    public async Task<Result> RollbackTransactionAsync()
    {
        if (_transaction == null)
        {
            return Result.Fail("No transaction to rollback");
        }
        try
        {
            await _transaction.RollbackAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to rollback transaction").CausedBy(ex));
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        return Result.Ok();
    }

    public async Task<Result> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DBConcurrencyException ex)
        {
            return Result.Fail(new Error("Concurrency conflict").CausedBy(ex));
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail(new Error("Database update error").CausedBy(ex));
        }
        catch (PostgresException ex)
        {
            return Result.Fail(new Error(ex.Message).CausedBy(ex));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Unexpected error").CausedBy(ex));
        }
        return Result.Ok();
    }
}
