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
    [Route("v1")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext db)
        {
            var emnployee = new User { Id = 1, UserName = "robin", Password = "robin", Role = "employee" };
            var manager = new User { Id = 2, UserName = "batman", Password = "batman", Role = "manager" };
            var category = new Category { Id = 1, Title = "Informatica" };
            var product = new Product { Id = 1, Category = category, Title = "mouse", Description = "Mouse Microsoft", Price = 299 };

            db.Users.Add(emnployee);
            db.Users.Add(manager);
            db.Categories.Add(category);
            db.Products.Add(product);

            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Dados configurados"
            });
        }

    }
}