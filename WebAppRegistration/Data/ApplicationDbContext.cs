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
    }
}