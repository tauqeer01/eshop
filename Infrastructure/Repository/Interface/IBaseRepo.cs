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
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(int id);
}
