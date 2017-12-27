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
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            EntityEntry<User> loggedUser;

            try
            {

                var authenticatedUser = await _magento.AuthenticateUser(new Models.User { Email = credentials.Email, Password = credentials.Password });
                if (authenticatedUser.Token == null)
                {
                    return NoContent();
                }

                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == credentials.Email);
                if (user == null)
                    loggedUser = await _dbContext.Users.AddAsync(authenticatedUser);
                else
                {

                    user.Name = authenticatedUser.Name;
                    user.LastName = authenticatedUser.LastName;
                    user.Token = authenticatedUser.Token;
                    user.Cpf = authenticatedUser.Cpf;
                    user.Email = authenticatedUser.Email;

                    loggedUser = _dbContext.Users.Update(user);
                }

                await _dbContext.SaveChangesAsync();

                return Ok(loggedUser.Entity);

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

            if (string.IsNullOrEmpty(user.Token))
            {
                return NotFound(user.Name);
            }

            user.Id = 0;

            var newUser = _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok(newUser.Entity);
        }
    }
}
