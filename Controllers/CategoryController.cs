using System.Collections.Generic;
using System.Threading.Tasks;
using Api_Shop.Data;
using Api_Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Shop.Controllers
{
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] -- para métodos que não tenham cache
        // usado quando no startup.cs, tiver usado services.AddResponseCaching();
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext db)
        {
            var categories = await db.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(
            int id,
            [FromServices] DataContext db)
        {
            var category = await db
                            .Categories
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Posts(
            [FromBody] Category model,
            [FromServices] DataContext db)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                db.Categories.Add(model);
                await db.SaveChangesAsync();
                return Ok(model);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = $"ERRO! Não foi possível criar categoria: {ex.Message}" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Put(
            int id,
            [FromServices] DataContext db,
            [FromBody] Category model)
        {
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada!" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                db.Entry<Category>(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateConcurrencyException db_ex)
            {
                return BadRequest(new { message = $"ERRO! Registro atualizado anteriormente, tente novamente depois: {db_ex.Message}" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = $"ERRO! Não foi possível atualizar categoria: {ex.Message}" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Delete([FromServices] DataContext db, int id)
        {
            var category = await db
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return NotFound(new { message = "Categoria não encontrada!" });
            }

            try
            {
                db.Categories.Remove(category);
                await db.SaveChangesAsync();
                return Ok(new { message = "Categoria removida!" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = $"Não foi possivel remover a categoria devido ao erro {ex.Message}" });
            }
        }
    }
}