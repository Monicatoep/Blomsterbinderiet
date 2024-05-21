using Blomsterbinderiet.Models;
using Microsoft.EntityFrameworkCore;

namespace Blomsterbinderiet.EFDbContext
{
    public class BlomstDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Data Source=mssql7.unoeuro.com;Initial Catalog=tinylink_se_db_blomsterbinderiet;User ID=tinylink_se;Password=dAp6gFkE93wnzmy5Bhxe;TrustServerCertificate=true");
            //options.LogTo(Console.WriteLine, LogLevel.Information);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

    }
}
