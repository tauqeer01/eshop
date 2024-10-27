using System;
using Core.Entities;

namespace Infrastructure.Repository.Interface;

public interface IUnitOfWork : IDisposable
{
    IBaseRepo<T> BaseRepo<T>() where T : BaseEntities;
    Task<bool> Complete();
}
