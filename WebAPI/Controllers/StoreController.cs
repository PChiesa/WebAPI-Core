using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class StoreController : Controller
    {

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody]Store store)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]Store store)
        {
            return Ok();
        }

    }
}