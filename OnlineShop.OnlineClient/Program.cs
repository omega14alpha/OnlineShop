using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineShop.DataAccess.Contexts;
using OnlineShop.OnlineClient.Identity;
using OnlineShop.OnlineClient.Identity.Models;
using OnlineShop.OrderArchiver.Models;
using System;
using System.Threading.Tasks;

namespace OnlineShop.OnlineClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<UserModel>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await RoleInitializer.InitializerAsync(userManager, rolesManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error accurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                    webBuilder.UseStartup<Startup>();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(options =>
                {
                    var connectionStrong = context.Configuration["AppConfig:ConnectionStrings:OnlineShopConnectionString"];
                    return new DbContextOptionsBuilder<DbOrderContext>().UseSqlServer(connectionStrong).Options;
                }).AddHostedService<Worker>();
                services.Configure<FoldersInfoModel>(context.Configuration.GetSection("AppConfig:Folders"));
            });
    }
}
