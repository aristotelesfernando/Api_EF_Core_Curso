using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_Shop.Data;
using Api_Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Shop.Controllers
{
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        #region Todas as maneiras possíveis de listar produtos
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext db)
        {
            var products = await db.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [Authorize(Roles = "employee")]
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

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Product>> Put(
            [FromServices] DataContext db,
            int id,
            [FromBody] Product model)
        {
            if (id != model.Id)
                return NotFound(new { message = "Produto não encontrada!" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                db.Entry<Product>(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateConcurrencyException db_ex)
            {
                return BadRequest(new { message = $"ERRO! Registro atualizado anteriormente, tente novamente depois: {db_ex.Message}" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = $"ERRO! Não foi possível atualizar produto: {ex.Message}" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Product>> Delete(
            int id,
            [FromServices] DataContext db)
        {
            var product = await db
                .Products
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound(new { message = "Produto não encontrada!" });
            }

            try
            {
                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return Ok(new { message = "Produto removido!" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = $"ERRO! Não foi possível remover a categoria: {ex.Message}" });
            }
        }
    }
}