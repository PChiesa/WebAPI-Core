using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebAPI.Database;
using WebAPI.Magento;

namespace WebAPI.Authorization
{
    public class UserAuthorization : IAuthorizationFilter
    {
        private readonly WebApiContext _context;
        private readonly IMagentoApi _magento;


        public UserAuthorization(WebApiContext context, IMagentoApi magento)
        {
            _context = context;
            _magento = magento;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var userToken = context.HttpContext.Request.Headers["UserToken"];


                var user = await _context.Users.FirstAsync(x => x.Token == userToken);

                var authenticatedUser = await _magento.AuthenticateUser(user);
                if (authenticatedUser == null)
                    context.Result = new UnauthorizedResult();

            }
            catch (InvalidOperationException)
            {
                context.Result = new UnauthorizedResult();
            }
            catch (Exception)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}