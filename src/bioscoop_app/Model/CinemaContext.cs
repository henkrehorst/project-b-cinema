using System;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class CinemaContext : DbContext
    {
        public DbSet<Kijkwijzer> Kijkwijzers { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ScreenTime> ScreenTimes { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=cinema.db");
        }
    }
}