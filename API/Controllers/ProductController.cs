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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IBaseRepo<Product> repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,
         string? type, string? sort)
        {
            var spec = new ProductSpecification(brand, type, sort);
            var products = await repo.ListAsync(spec);
            return Ok(products);
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
            return Ok();
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetTypes()
        {
            return Ok();
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
