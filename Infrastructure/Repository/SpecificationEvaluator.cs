using System;
using Core.Entities;
using Core.Interface;
using Infrastructure.Repository.Interface;

namespace Infrastructure.Repository;

public class SpecificationEvaluator<T> where T : BaseEntities
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecifications<T> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }
        return query;
    }
}
