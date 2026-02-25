using System.ComponentModel.DataAnnotations;

namespace MovieRatingAPI.Models.DTO
{
	public class UpdateMovieRequestDto
	{
		[Required]
		[MinLength(2, ErrorMessage = "Title has to be a minimum of 2 characters")]
		[MaxLength(25, ErrorMessage = "Title has to be a maximum of 25 characters")]
		public string Title { get; set; }

		[Required]
		[MaxLength(100, ErrorMessage = "Title has to be a maximum of 100 characters")]
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public Guid CreatedBy { get; set; }
	}
}
