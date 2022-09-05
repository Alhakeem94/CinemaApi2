using CinemaApi2.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApi2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {

        }

        public DbSet<MoviesModel> MoviesTable { get; set; }

    }
}
