using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Authorization;
using WebAPI.Database;
using WebAPI.Magento;
using WebAPI.Services;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<WebApiContext>();
            services.AddScoped<StoreAuthorization>();
            services.AddScoped<UserAuthorization>();
            services.AddScoped<AppVersionAuthorization>();
            services.AddSingleton<AppSettings>((s) => Configuration.GetSection("App").Get<AppSettings>());
            services.AddTransient<IMagentoApi, MagentoApi>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseCors((p) =>
                {
                    p.AllowAnyHeader();
                    p.AllowAnyMethod();
                    p.AllowAnyOrigin();
                });
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
