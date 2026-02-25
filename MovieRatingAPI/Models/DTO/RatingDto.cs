namespace MovieRatingAPI.Models.DTO
{
	public class RatingDto
	{
		public Guid Id { get; set; }
		public Guid MovieId { get; set; }
		public Guid UserId { get; set; }
		public int Score { get; set; }
		public string? Comment { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
