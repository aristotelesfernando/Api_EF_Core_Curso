using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_Shop.Data;
using Api_Shop.Models;
using Api_Shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Shop.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext db)
        {
            var users = await db.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("")]
        //[Authorize(Roles = "manager")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
                    [FromServices] DataContext db,
                    [FromBody] User model
                )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                model.Role = "employee";
                db.Users.Add(model);
                await db.SaveChangesAsync();

                model.Password = "";
                return Ok(model);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = $"Não foi possível criar usuário! Erro: {ex.Message}" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(
            [FromServices] DataContext db,
            int id,
            [FromBody] User model
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return NotFound(new { message = "Usuário não encontrado!" });
            }

            try
            {
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(model);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = $"Não foi possível criar o usuário. Erro: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromServices] DataContext db,
            [FromBody] User model
        )
        {
            var user = await db.Users
                .AsNoTracking()
                .Where(x => x.UserName == model.UserName && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "Usuário ou senha invalidos!" });
            }

            var token = TokenService.GenerateToken(user);

            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

    }
}