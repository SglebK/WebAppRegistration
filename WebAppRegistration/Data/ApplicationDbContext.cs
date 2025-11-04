using Microsoft.EntityFrameworkCore;
using WebAppRegistration.Models;

namespace WebAppRegistration.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasQueryFilter(u => u.DateDeleted == null);
            modelBuilder.Entity<CartItem>().HasQueryFilter(ci => ci.User.DateDeleted == null);
            modelBuilder.Entity<Order>().HasQueryFilter(o => o.User.DateDeleted == null);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(oi => oi.Order.User.DateDeleted == null);
        }
    }
}