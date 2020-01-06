using FoodAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodAdvisor.Services
{
    public class RestaurantContext : DbContext
    {
        //public RestaurantContext(DbContextOptions options)
        //    : base(options)
        //{ }

        private string connectionString { get; set; }

        public RestaurantContext()
        {
            connectionString = @"server=Poulpe;database=FoodAdvisor;trusted_connection=true;";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }
}
