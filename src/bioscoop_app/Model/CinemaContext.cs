using Microsoft.EntityFrameworkCore;

namespace bioscoop_app.Model
{
    public class CinemaContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=cinema.db");
        }
    }
}