using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieRatingAPI.Data
{
	public class MovieRatingAuthDbContext : IdentityDbContext
	{
		public MovieRatingAuthDbContext(DbContextOptions<MovieRatingAuthDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var readerRoleId = "fe58bdc7-4d36-49ca-8c1d-c53bed6267c8";
			var writerRoleId = "a376765d-79e5-4ab0-98ca-e821fb343b9b";

			var roles = new List<IdentityRole>
			{
				new IdentityRole
				{
					Id = readerRoleId,
					ConcurrencyStamp = readerRoleId,
					Name = "Reader",
					NormalizedName = "Reader".ToUpper()
				},
				new IdentityRole
				{
					Id = writerRoleId,
					ConcurrencyStamp = writerRoleId,
					Name = "Writer",
					NormalizedName = "Writer".ToUpper()
				},
			};

			builder.Entity<IdentityRole>().HasData(roles);
		}
	}
}
