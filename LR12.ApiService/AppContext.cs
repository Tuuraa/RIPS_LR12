using Microsoft.EntityFrameworkCore;

namespace LR12.ApiService
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ComputerComponent> ComputerComponents { get; set; } = null!;
        public DbSet<Sale> Sales { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}