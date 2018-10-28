using Microsoft.EntityFrameworkCore;

namespace FirstApp
{
     public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
    }
}