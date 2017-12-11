using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Database;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VoucherController : Controller
    {
        private readonly WebApiContext _dbContext;

        public VoucherController(WebApiContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{userId}")]
        public IActionResult GetEvents(int userId)
        {
            return Ok(new List<Event>
            {
                new Event{ Name = "Event1" },
                new Event{ Name = "Event2" },
                new Event{ Name = "Event3" },
                new Event{ Name = "Event4" },
                new Event{ Name = "Event5" },
                new Event{ Name = "Event6" },
                new Event{ Name = "Event7" },
                new Event{ Name = "Event8" },
                new Event{ Name = "Event9" },
                new Event{ Name = "Event10" },
            });
        }

        [HttpGet("{eventId}/{userId}")]
        public IActionResult GetVouchers(int eventId, int userId)
        {
            return Ok(new List<Voucher>
                {
                    new Voucher { Id = 1, Token = "1234"},
                    new Voucher { Id = 2, Token = "1234"},
                    new Voucher { Id = 3, Token = "1234"},
                    new Voucher { Id = 4, Token = "1234"},
                });
        }

        [HttpPost]
        public IActionResult CreateVoucher([FromBody] Voucher voucher)
        {
            return Ok();
        }
    }
}
