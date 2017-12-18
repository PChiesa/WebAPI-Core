using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authorization;
using WebAPI.Database;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class StoreController : Controller
    {
        private readonly WebApiContext _context;

        public StoreController(WebApiContext context)
        {
            _context = context;
        }

        [ServiceFilter(typeof(StoreAuthorization))]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var storeId = int.Parse(Request.Headers["StoreId"]);
            var store = await _context.Stores.FindAsync(storeId);

            if (store == null)
                return Ok();

            return Ok(new { Id = store.Id, ClientStoreId = store.ClientStoreId, Name = store.Name, Hash = store.Hash });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Store store)
        {
            try
            {
                var secretKey = new SecureRandomString().Generate(12);
                var hash = new HashStringService().Hash(store.Name + secretKey);

                store.Hash = hash;
                store.SecretKey = secretKey;

                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();

                return Ok(hash);
            }
            catch (System.Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [ServiceFilter(typeof(StoreAuthorization))]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Store store)
        {
            try
            {
                var storeId = int.Parse(Request.Headers["StoreId"]);
                var oldStore = await _context.Stores.FindAsync(storeId);
                oldStore.Name = store.Name;

                _context.Stores.Update(oldStore);
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