using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Authorization;
using WebAPI.Database;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ServiceFilter(typeof(StoreAuthorization))]
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private readonly WebApiContext _context;

        public EventController(WebApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var storeId = int.Parse(Request.Headers["StoreId"]);
            var events = await _context.Events.Where(x => x.StoreId == storeId).ToListAsync();
            return Ok();
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var storeId = int.Parse(Request.Headers["StoreId"]);
                var events = await _context.Events.FirstAsync(x => x.StoreId == storeId && x.Id == id);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return Ok();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Event e)
        {
            try
            {
                var storeId = int.Parse(Request.Headers["StoreId"]);
                e.Id = 0;
                e.StoreId = storeId;

                await _context.Events.AddAsync(e);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Event e)
        {
            try
            {
                var storeId = int.Parse(Request.Headers["StoreId"]);

                var oldEvent = await _context.Events.FirstAsync(ev => ev.StoreId == storeId && (ev.Id == e.Id || ev.ClientEventId == e.ClientEventId));

                oldEvent.Name = e.Name;
                oldEvent.Date = e.Date;
                oldEvent.Description1 = e.Description1;
                oldEvent.Description2 = e.Description2;
                oldEvent.Description3 = e.Description3;
                oldEvent.Image1 = e.Image1;
                oldEvent.Image2 = e.Image2;
                oldEvent.Image3 = e.Image3;

                _context.Events.Update(e);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}