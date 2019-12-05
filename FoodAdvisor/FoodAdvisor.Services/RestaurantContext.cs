using FoodAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodAdvisor.Services
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions options)
            : base(options)
        { }
        
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }
}
