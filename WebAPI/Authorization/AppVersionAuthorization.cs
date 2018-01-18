using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Services;

namespace WebAPI.Authorization
{
    public class AppVersionAuthorization : IAuthorizationFilter
    {
        private readonly AppSettings _app;
        public AppVersionAuthorization(AppSettings app)
        {
            _app = app;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var appVersion = int.Parse(context.HttpContext.Request.Headers["AppVersion"].ToString());
                if (appVersion < _app.MinimumAppVersion)
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.UpgradeRequired);
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
