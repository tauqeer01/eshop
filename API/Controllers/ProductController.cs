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

    public class ProductController(IUnitOfWork unit) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);
            return await CreatedPageResult(unit.BaseRepo<Product>(), spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await unit.BaseRepo<Product>().GetByIdAsync(id);

            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            unit.BaseRepo<Product>().Add(product);
            await unit.BaseRepo<Product>().SaveAllAsync();
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> PutProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            unit.BaseRepo<Product>().Update(product);

            try
            {
                await unit.Complete();
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
            return Ok(await unit.BaseRepo<Product>().ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetTypes()
        {
            var spec = new TypesListSpecification();
            return Ok(await unit.BaseRepo<Product>().ListAsync(spec));

        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await unit.BaseRepo<Product>().GetByIdAsync(id);
            if (product == null) return NotFound();

            unit.BaseRepo<Product>().Remove(product);
            await unit.BaseRepo<Product>().SaveAllAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return unit.BaseRepo<Product>().Exists(id);
        }
    }
}
