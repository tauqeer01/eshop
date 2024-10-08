using API.RequestHelper;
using Core.Entities;
using Core.Specifications;
using Infrastructure.Data;
using Infrastructure.Repository;
using Infrastructure.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class ProductController(IBaseRepo<Product> repo) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);
            return await CreatedPageResult(repo, spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);

            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            repo.Add(product);
            await repo.SaveAllAsync();
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> PutProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            repo.Update(product);

            try
            {
                await repo.SaveAllAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpGet("brand")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetBrands()
        {
            var spec = new BrandListSpec();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetTypes()
        {
            var spec = new TypesListSpecification();
            return Ok(await repo.ListAsync(spec));

        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null) return NotFound();

            repo.Remove(product);
            await repo.SaveAllAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return repo.Exists(id);
        }
    }
}
