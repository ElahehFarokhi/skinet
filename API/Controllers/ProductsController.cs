using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IGenericRepository<Product> productRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);
            var products = await productRepository.ListAsync(spec);
            var count = await productRepository.CountAsync(spec);
            var pagination = new Pagination<Product>(specParams.PageIndex,
                specParams.PageSize, count, products);

            return Ok(pagination);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            productRepository.Add(product);

            if (await productRepository.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }

            return BadRequest("Problem creating product.");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !ProductExists(id)) return BadRequest("Cannot update this product");

            productRepository.Update(product);
            if (await productRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating the product");
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            productRepository.Delete(product);
            if (await productRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await productRepository.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await productRepository.ListAsync(spec));
        }


        private bool ProductExists(int id)
        {
            return productRepository.Exists(id);
        }

    }
}
