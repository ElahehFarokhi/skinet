using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(StoreContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return product;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if ( id != product.Id || !await ProductExists(id)) return BadRequest("Cannot update this product");

            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();            
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null) return NotFound();
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return NoContent();            
        }

        private async Task<bool> ProductExists(int id) 
        {
            return await context.Products.AnyAsync(x => x.Id == id);
        }

    }
}
