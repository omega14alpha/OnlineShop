using Microsoft.EntityFrameworkCore;
using OnlineShop.DataAccess.Entities;

namespace OnlineShop.DataAccess.Contexts
{
    public class DbOrderContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbOrderContext(DbContextOptions<DbOrderContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasKey(i => i.Id);
            modelBuilder.Entity<Client>().Property(n => n.Name).HasMaxLength(20).IsRequired();
            modelBuilder.Entity<Client>().HasIndex(n => n.Name).IsUnique();

            modelBuilder.Entity<Item>().HasKey(i => i.Id);
            modelBuilder.Entity<Item>().Property(n => n.Name).HasMaxLength(20).IsRequired();
            modelBuilder.Entity<Item>().HasIndex(n => n.Name).IsUnique();

            modelBuilder.Entity<Manager>().HasKey(i => i.Id);
            modelBuilder.Entity<Manager>().Property(s => s.Surname).HasMaxLength(20).IsRequired();
            modelBuilder.Entity<Manager>().HasIndex(s => s.Surname).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
