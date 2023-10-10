using Microsoft.EntityFrameworkCore;

namespace Iot.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<DeviceData> Devices { get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=iot_db;Username=postgres;Password=12345");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceData>(builder =>
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
