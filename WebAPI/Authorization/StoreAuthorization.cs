using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Database;
using WebAPI.Services;

namespace WebAPI.Authorization
{
    public class StoreAuthorization : IAuthorizationFilter
    {
        private readonly WebApiContext _context;
        public StoreAuthorization(WebApiContext context)
        {
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var storeToken = context.HttpContext.Request.Headers["StoreToken"];
                var store = _context.Stores.First(x => x.Hash == storeToken);

                if (storeToken != new HashStringService().Hash(store.Name + store.SecretKey))
                    context.Result = new UnauthorizedResult();
                else
                    context.HttpContext.Request.Headers.Add("StoreId", store.Id.ToString());
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