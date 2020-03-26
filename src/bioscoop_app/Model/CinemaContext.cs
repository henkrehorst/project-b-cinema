using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace bioscoop_app.Model
{
    public class CinemaContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Ticket> tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySQL("server=bioscoop-makkers.henkrehorst.nl;database=develop-henk;user=henk;password=oLcD9ZBiFd4qLj1x");
    }
}