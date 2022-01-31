using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.OnlineClient.Identity.Models;

namespace OnlineShop.OnlineClient.Identity.Contexts
{
    internal class DbIdentityContext : IdentityDbContext<UserModel>
    {
        public DbIdentityContext(DbContextOptions<DbIdentityContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
