using System;
using API.RequestHelper;
using Core.Entities;
using Core.Interface;
using Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    protected async Task<ActionResult> CreatedPageResult<T>(IBaseRepo<T> repo,
     ISpecifications<T> spec, int pageIndex, int pageSize) where T : BaseEntities
    {
        var items = await repo.ListAsync(spec);
        var count = await repo.CountAsync(spec);
        var pagination = new Pagination<T>(pageIndex, pageSize, count, items);
        return Ok(pagination);
    }
}
