using System;
using System.Threading.Tasks;

namespace Tilegram.Feature.Profile
{
	public interface IProfileService
	{
		Task<Either<Exception, ProfileData>> GetProfileData(string username);
		Task<Either<Exception, PostsResponse>> GetProfilePosts(string username, int pageToLoad = 5);
	}
}