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
using WebAPI.Enums;

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


        [ServiceFilter(typeof(StoreAuthorization))]
        [HttpGet("{voucherId}/{voucherStatus}")]
        public async Task<IActionResult> UpdateVoucherStatus(int voucherId, VoucherStatus voucherStatus)
        {
            try
            {
                var voucher = await _dbContext.Vouchers.FirstAsync(x => x.Id == voucherId);
                voucher.CurrentStatus = voucherStatus;
                await _dbContext.SaveChangesAsync();

                return Ok();

            }
            catch (InvalidOperationException)
            {
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [ServiceFilter(typeof(StoreAuthorization))]
        [HttpGet("{clientEventId}/{lastVoucherId}")]
        public async Task<IActionResult> GetVouchersForLocal(string clientEventId, int lastVoucherId)
        {
            try
            {
                var vouchers = await _dbContext.Vouchers
                    .OrderBy(x => x.Id)
                    .Where(x => x.ClientEventId == clientEventId && x.Id > lastVoucherId)
                    .ToListAsync();

                if (!vouchers.Any())
                    return NoContent();

                return Ok(vouchers);

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [ServiceFilter(typeof(UserAuthorization))]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetEvents(int userId)
        {

            var events = await _dbContext.Vouchers
                .Where(x => x.UserId == userId && x.Event.Date > DateTime.Now)
                .GroupBy(x => x.Event)
                .Select(x => x.Key)
                .ToListAsync();


            events.ForEach(x =>
            {
                var store = _dbContext.Stores.Find(x.StoreId);

                x.Store = new Store { Id = store.Id, Name = store.Name };
            });

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

        [ServiceFilter(typeof(StoreAuthorization))]
        [HttpPost]
        public async Task<IActionResult> CreateVoucherList([FromBody] Voucher[] voucher)
        {
            List<Voucher> failedVouchers = new List<Voucher>();
            int length = voucher.Count();

            try
            {
                for (int i = 0; i < length; i++)
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        var currentVoucher = voucher[i];

                        try
                        {
                            /*
                             Check if the user of the voucher already exists in the voucher seguro API database
                             */
                            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.ClientUserId == currentVoucher.ClientUserId);
                            int? userId = user?.Id;
                            if (user == null)
                            {
                                /*
                                 If the user doesn't exists create a new user with the data from the voucher to allow the user to login on the mobile APP
                                 */

                                var newUser = await _dbContext.Users.AddAsync(
                                    new Models.User
                                    {
                                        ClientUserId = currentVoucher.ClientUserId,
                                        Cpf = currentVoucher.ClientUserCpf,
                                        Email = currentVoucher.ClientUserEmail
                                    });

                                await _dbContext.SaveChangesAsync();

                                userId = newUser.Entity.Id;
                            }

                            var ev = await _dbContext.Events.FirstAsync(x => x.ClientEventId == currentVoucher.ClientEventId);

                            /*Updating the voucher with the event and user data*/
                            currentVoucher.EventId = ev.Id;
                            currentVoucher.CurrentStatus = Enums.VoucherStatus.Active;
                            currentVoucher.Token = new SecureRandomString().Generate(8);
                            currentVoucher.Id = 0;
                            currentVoucher.UserId = (int)userId;

                            await _dbContext.Vouchers.AddAsync(currentVoucher);
                            await _dbContext.SaveChangesAsync();

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            failedVouchers.Add(currentVoucher);
                        }
                    }/*<-- using*/
                }/*<-- for*/
                return Ok(failedVouchers);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
