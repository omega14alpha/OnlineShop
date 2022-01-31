using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using OnlineShop.BusinessLogic.Interfaces;
using OnlineShop.BusinessLogic;
using OnlineShop.DataAccess.Contexts;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using OnlineShop.OnlineClient.Identity.Contexts;
using OnlineShop.OnlineClient.Identity.Models;

namespace OnlineShop.OnlineClient
{
    public class Startup
    {
        public readonly IConfiguration _configuration;

        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDbWorker, DbWorker>();
            services.AddDbContext<DbOrderContext>().AddDbContext<DbIdentityContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(_configuration["AppConfig:ConnectionStrings:OnlineShopConnectionString"]);
                options.UseSqlServer(_configuration["AppConfig:ConnectionStrings:OnlineUserShopConnectionString"]);
            });

            services.AddIdentity<UserModel, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DbIdentityContext>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
