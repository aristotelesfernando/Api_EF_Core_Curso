using System.Collections.Generic;
using System.Threading.Tasks;
using Api_Shop.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api_Shop.Controllers
{
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return new List<Category>();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            return new Category();
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Category>> Posts([FromBody] Category model)
        {
            return Ok(model);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model)
        {
            if (model.Id == id)
            {
                return Ok(model);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("")]
        public async Task<ActionResult<Category>> Delete()
        {
            return Ok();
        }
    }
}