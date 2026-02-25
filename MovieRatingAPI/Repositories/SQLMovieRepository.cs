using Microsoft.EntityFrameworkCore;
using MovieRatingAPI.Data;
using MovieRatingAPI.Models.Domain;
using MovieRatingAPI.Models.DTO;

namespace MovieRatingAPI.Repositories
{
	public class SQLMovieRepository : IMovieRepository
	{
		private readonly MovieRatingDbContext dbContext;

		public SQLMovieRepository(MovieRatingDbContext dbContext)
        {
			this.dbContext = dbContext;
		}

		public async Task<Movie> CreateAsync(Movie movie)
		{
			await dbContext.Movies.AddAsync(movie);
			await dbContext.SaveChangesAsync();
			return movie;
		}

		public async Task<Movie?> DeleteAsync(Guid id)
		{
			var existingMovie = await dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);

			if (existingMovie == null)
			{
				return null;
			}

			dbContext.Movies.Remove(existingMovie);
			await dbContext.SaveChangesAsync();

			return existingMovie;
		}

		public async Task<List<Movie>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
			string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10)
		{
			var movies = dbContext.Movies.AsQueryable();

			// Filtering
			if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
			{
				if (filterOn.Equals("Title", StringComparison.OrdinalIgnoreCase))
				{
					movies = movies.Where(x => x.Title.Contains(filterQuery));
				}
			}

			// Sorting
			if (string.IsNullOrWhiteSpace(sortBy) == false)
			{
				if (sortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
				{
					movies = isAscending ? movies.OrderBy(x => x.Title) : movies.OrderByDescending(x => x.Title);
				}
			}

			// Pagination
			var skipResults = (pageNumber - 1) * pageSize;

			return await movies.Skip(skipResults).Take(pageSize).ToListAsync();
		}

		public async Task<Movie?> GetByIdAsync(Guid id)
		{
			return await dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Movie?> UpdateAsync(Guid id, Movie movie)
		{
			var existingMovie = await dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);

			if (existingMovie == null)
			{
				return null;
			}

			existingMovie.Title = movie.Title;
			existingMovie.Description = movie.Description;
			existingMovie.CreatedBy = movie.CreatedBy;
			existingMovie.CreatedAt = movie.CreatedAt;

			await dbContext.SaveChangesAsync();
			return existingMovie;
		}
	}
}
