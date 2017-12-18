using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Database;
using WebAPI.Magento;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly WebApiContext _dbContext;
        private readonly IMagentoApi _magento;
        public LoginController(WebApiContext dbContext, IMagentoApi magento)
        {
            _dbContext = dbContext;
            _magento = magento;
        }

        [HttpPost]
        public async Task<IActionResult> LogUser([FromBody]LoginCredentials credentials)
        {
            try
            {
                var user = await _dbContext.Users.FirstAsync(x => x.Cpf == credentials.Cpf || x.Email == credentials.Email);
                user.Password = credentials.Password;

                var authenticatedUser = await _magento.AuthenticateUser(user);
                if (authenticatedUser == null)
                {
                    return NoContent();
                }

                user.Token = authenticatedUser.Token;
                var loggedUser = _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                return Ok(loggedUser.Entity);

            }
            catch (InvalidOperationException)
            {
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{email}")]
        public IActionResult RecoverPassword(string email)
        {

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]Models.User user)
        {
            user = await _magento.RegisterUser(user);
            user.Id = 0;

            var newUser = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return Ok(newUser.Entity);
        }
    }
}
