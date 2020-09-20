using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_Shop.Data;
using Api_Shop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Shop.Controllers
{
    [Route("products")]
    public class ProductController : ControllerBase
    {
        #region Todas as maneiras possíveis de listar produtos
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext db)
        {
            var products = await db.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext db, int id)
        {
            var product = await db.Products
                        .Include(x => x.Category)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext db, int id)
        {
            var products = await db.Products
                        .Include(x => x.Category)
                        .AsNoTracking()
                        .Where(x => x.CategoryId == id)
                        .ToListAsync();
            return products;
        }
        #endregion

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext db, [FromBody] Product model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Products.Add(model);
                    await db.SaveChangesAsync();
                    return Ok(model);
                }
                catch (System.Exception ex)
                {
                    return BadRequest(new { message = $"Ocorreu um erro ao criar um novo produto. Erro: {ex.Message}" });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}