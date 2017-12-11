using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Database;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly WebApiContext _dbContext;
        public LoginController(WebApiContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult LogUser([FromBody]LoginCredentials credentials)
        {
            //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //return null;
            return Ok(new User { Id = 1, Cpf = credentials.Cpf, Email = credentials.Email, Password = credentials.Password });
        }

        [HttpGet("{email}")]
        public IActionResult RecoverPassword(string email)
        {

            return NoContent();
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody]User user)
        {
            user.Token = Guid.NewGuid().ToString("N");
            return Ok(user);
        }
    }
}
