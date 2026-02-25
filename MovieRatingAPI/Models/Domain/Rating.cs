namespace MovieRatingAPI.Models.Domain
{
	public class Rating
	{
        public Guid Id { get; set; }
		public Guid MovieId { get; set; }
		public Guid UserId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public Movie Movie { get; set; }
    }
}
