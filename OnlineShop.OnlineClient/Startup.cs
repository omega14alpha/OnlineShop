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
using OnlineShop.BusinessLogic.Models;
using OnlineShop.DataAccess;
using Microsoft.Extensions.Logging;
using OnlineShop.OnlineClient.Infrastructure.Logger;

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
            services.AddDbContext<DbOrderContext>().AddDbContext<DbIdentityContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(_configuration["AppConfig:ConnectionStrings:OnlineShopConnectionString"]);
                options.UseSqlServer(_configuration["AppConfig:ConnectionStrings:OnlineUserShopConnectionString"]);
            });

            services.AddScoped(provider => new DataBaseUoW(provider.GetRequiredService<DbOrderContext>()));
            services.AddScoped<IModelWorker<OrderModel>>(provider => new OrderWorker(provider.GetRequiredService<DataBaseUoW>()));
            services.AddScoped<IModelWorker<ManagerModel>>(provider => new ManagerWorker(provider.GetRequiredService<DataBaseUoW>()));
            services.AddScoped<IModelWorker<ClientModel>>(provider => new ClientWorker(provider.GetRequiredService<DataBaseUoW>()));
            services.AddScoped<IModelWorker<ItemModel>>(provider => new ItemWorker(provider.GetRequiredService<DataBaseUoW>()));

            services.AddIdentity<UserModel, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DbIdentityContext>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var filePath = _configuration["LoggerFilePaths:OnlineClientFilePath"];
            loggerFactory.AddFile(filePath);

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
