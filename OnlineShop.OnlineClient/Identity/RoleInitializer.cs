using Microsoft.AspNetCore.Identity;
using OnlineShop.OnlineClient.Identity.Models;
using System.Threading.Tasks;

namespace OnlineShop.OnlineClient.Identity
{
    internal class RoleInitializer
    {
        public static async Task InitializerAsync(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@google.com";
            string password = "_Aa123456";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                UserModel admin = new UserModel() { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
