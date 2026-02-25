using MovieRatingAPI.Models.Domain;
using MovieRatingAPI.Models.DTO;

namespace MovieRatingAPI.Repositories
{
	public interface IMovieRepository
	{
		Task<List<Movie>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
			string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10);
		Task<Movie?> GetByIdAsync(Guid id);

		Task<Movie> CreateAsync(Movie movie);

		Task<Movie?> UpdateAsync(Guid id, Movie movie);

		Task<Movie?> DeleteAsync(Guid id);
	}
}
