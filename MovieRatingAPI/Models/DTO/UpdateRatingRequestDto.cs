using System.ComponentModel.DataAnnotations;

namespace MovieRatingAPI.Models.DTO
{
	public class UpdateRatingRequestDto
	{
		[Required]
		public Guid MovieId { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[Range(0, 10, ErrorMessage = "Score must be between 0 and 10.")]
		public int Score { get; set; }

		[MaxLength(100, ErrorMessage = "Comment has to be a maximum of 100 characters")]
		public string? Comment { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
