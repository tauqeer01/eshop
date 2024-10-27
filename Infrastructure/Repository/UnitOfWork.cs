using System;
using System.Collections.Concurrent;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repository.Interface;

namespace Infrastructure.Repository;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repo = new();
    public  IBaseRepo<T> BaseRepo<T>() where T : BaseEntities
    {
        var type = typeof(T).Name;
        return (IBaseRepo<T>)_repo.GetOrAdd(type, t =>
        {
            var repoType = typeof(BaseRepo<>).MakeGenericType(typeof(T));
            return Activator.CreateInstance(repoType, context)
            ?? throw new InvalidOperationException($"Could not create repo instance for {t}");
        });
    }

    public async Task<bool> Complete()
    {
         return await context.SaveChangesAsync() > 0;
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
