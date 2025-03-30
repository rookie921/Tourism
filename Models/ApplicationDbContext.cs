using Microsoft.EntityFrameworkCore;

namespace tourism02.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Promotion> Promotions { get; set; }

        public DbSet<Booking> Bookings { get; set; }

    }
}
