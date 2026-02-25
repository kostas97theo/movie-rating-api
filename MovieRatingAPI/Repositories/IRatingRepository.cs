using MovieRatingAPI.Models.Domain;

namespace MovieRatingAPI.Repositories
{
	public interface IRatingRepository
	{
		Task<List<Rating>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
			string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10);
		Task<Rating?> GetByIdAsync(Guid id);
		Task<Rating> CreateAsync(Rating rating);
		Task<Rating?> DeleteAsync(Guid id);
		Task<Rating?> UpdateAsync(Guid id, Rating rating);
	}
}
