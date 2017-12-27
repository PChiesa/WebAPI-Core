using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Authorization;
using WebAPI.Database;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.Magento;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VoucherController : Controller
    {
        private readonly WebApiContext _dbContext;
        private readonly IMagentoApi _magentoApi;

        public VoucherController(WebApiContext dbContext, IMagentoApi magentoApi)
        {
            _dbContext = dbContext;
            _magentoApi = magentoApi;
        }

        [ServiceFilter(typeof(UserAuthorization))]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetEvents(int userId)
        {
            var events = await _dbContext.Vouchers.Where(x => x.UserId == userId && x.Event.Date > DateTime.Now).Select(x => x.Event).ToListAsync();

            return Ok(events);
        }

        [ServiceFilter(typeof(UserAuthorization))]
        [HttpGet("{eventId}/{userId}")]
        public async Task<IActionResult> GetVouchers(int eventId, int userId)
        {
            var vouchers = await _dbContext.Vouchers.Where(x => x.EventId == eventId && x.UserId == userId && x.ExpirationDate > DateTime.Now && x.CurrentStatus == Enums.VoucherStatus.Active).ToListAsync();
            return Ok(vouchers);
        }

        [ServiceFilter(typeof(StoreAuthorization))]
        [HttpPost]
        public async Task<IActionResult> CreateVoucher([FromBody] Voucher voucher)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.ClientUserId == voucher.ClientUserId);
                int userId = 0;
                if (user == null)
                {
                    user = await _magentoApi.GetUserInfo(int.Parse(voucher.ClientUserId));
                    if (user == null)
                        throw new InvalidOperationException();

                    var newUser = await _dbContext.Users.AddAsync(user);
                    await _dbContext.SaveChangesAsync();

                    userId = newUser.Entity.Id;
                }

                var ev = await _dbContext.Events.FirstAsync(x => x.ClientEventId == voucher.ClientEventId);

                voucher.EventId = ev.Id;
                voucher.CurrentStatus = Enums.VoucherStatus.Active;
                voucher.Token = new SecureRandomString().Generate(8);
                voucher.Id = 0;
                voucher.UserId = userId;

                await _dbContext.Vouchers.AddAsync(voucher);
                await _dbContext.SaveChangesAsync();

                return Ok();

            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
