using Microsoft.EntityFrameworkCore;
using MovieRatingAPI.Data;
using MovieRatingAPI.Models.Domain;

namespace MovieRatingAPI.Repositories
{
	public class SQLRatingRepository : IRatingRepository
	{
		private readonly MovieRatingDbContext dbContext;

		public SQLRatingRepository(MovieRatingDbContext dbContext)
        {
			this.dbContext = dbContext;
		}

		public async Task<Rating> CreateAsync(Rating rating)
		{
			await dbContext.Ratings.AddAsync(rating);
			await dbContext.SaveChangesAsync();
			return rating;
		}

		public async Task<Rating?> DeleteAsync(Guid id)
		{
			var existingRating = await dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == id);

			if (existingRating == null)
			{
				return null;
			}

			dbContext.Ratings.Remove(existingRating);
			await dbContext.SaveChangesAsync();
			return existingRating;
		}

		public async Task<List<Rating>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
			string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10)
		{
			var ratings = dbContext.Ratings.AsQueryable();

			// Filtering
			if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
			{
				ratings = ratings.Where(r => r.MovieId == Guid.Parse(filterQuery));
			}

			// Sorting
			if (string.IsNullOrWhiteSpace(sortBy) == false)
			{
				if (sortBy.Equals("MovieId", StringComparison.OrdinalIgnoreCase))
				{
					ratings = isAscending ? ratings.OrderBy(r => r.MovieId) : ratings.OrderByDescending(r => r.MovieId);
				}
				else if (sortBy.Equals("Score"))
				{
					ratings = isAscending ? ratings.OrderBy(r => r.Score) : ratings.OrderByDescending(r => r.Score);
				}
			}

			// Pagination
			var skipResults = (pageNumber - 1) * pageSize;

			return await ratings.Skip(skipResults).Take(pageSize).ToListAsync();
		}

		public async Task<Rating?> GetByIdAsync(Guid id)
		{
			return await dbContext.Ratings.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Rating?> UpdateAsync(Guid id, Rating rating)
		{
			var existingRating = await dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == id);

			if (existingRating == null)
			{
				return null;
			}

			existingRating.MovieId = rating.MovieId;
			existingRating.UserId = rating.UserId;
			existingRating.Score = rating.Score;
			existingRating.Comment = rating.Comment;
			existingRating.CreatedAt = rating.CreatedAt;

			await dbContext.SaveChangesAsync();
			return existingRating;
	}
	}
}
