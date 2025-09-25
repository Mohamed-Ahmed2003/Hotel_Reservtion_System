using Hotel_Reservtion_System.Entity;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Reservtion_System.DatabaseContext
{
    public class HoteldbContext : DbContext
    {
        public HoteldbContext(DbContextOptions<HoteldbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Branch> branches { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<Notification> notifcations { get; set; }
        public DbSet<Room> rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<Booking>().ToTable("Bookings");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Branch>().ToTable("Branches");
            modelBuilder.Entity<Invoice>().ToTable("Invoices");
            modelBuilder.Entity<Notification>().ToTable("Notifcations");
        }
    }
}
