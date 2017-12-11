using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class EventController : Controller
    {

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody]Event e)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody]Event e)
        {
            return Ok();
        }
    }
}