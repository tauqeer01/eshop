using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Repository.Interface;

public interface IBaseRepo<T> where T : BaseEntities
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetEntityWithSpec(ISpecifications<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecifications<T> spec);

    Task<TResult?> GetEntityWithSpec<TResult>(ISpecifications<T, TResult> spec);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecifications<T, TResult> spec);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(int id);
    Task<int> CountAsync(ISpecifications<T> spec);
}
