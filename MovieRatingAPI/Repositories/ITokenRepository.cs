using Microsoft.AspNetCore.Identity;

namespace MovieRatingAPI.Repositories
{
	public interface ITokenRepository
	{
		string CreateJWTToken(IdentityUser user, List<string> roles);
	}
}
