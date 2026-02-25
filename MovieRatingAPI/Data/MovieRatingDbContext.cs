using Microsoft.EntityFrameworkCore;
using MovieRatingAPI.Models.Domain;

namespace MovieRatingAPI.Data
{
	public class MovieRatingDbContext: DbContext
	{
		public MovieRatingDbContext(DbContextOptions<MovieRatingDbContext> dbContextOptions): base(dbContextOptions)
		{

		}

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
