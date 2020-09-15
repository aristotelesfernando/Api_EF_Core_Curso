using Microsoft.AspNetCore.Mvc;

namespace Api_Shop.Controllers
{
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "GET";
        }

        [HttpPost]
        [Route("")]
        public string Posts()
        {
            return "POST";
        }

        [HttpPut]
        [Route("")]
        public string Put()
        {
            return "PUT";
        }

        [HttpDelete]
        [Route("")]
        public string Delete()
        {
            return "DELETE";
        }
    }
}