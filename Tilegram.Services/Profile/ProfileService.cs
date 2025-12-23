using System;
using System.Threading.Tasks;

namespace Tilegram.Feature.Profile
{
    public class ProfileService : IProfileService
    {

        private const string GetProfilePath = "/api/Profile/{username}";
        private const string GetProfilePostsPath = "/api/Profile/posts/{username}?page={page}";

        private string ApiBaseUrl { get; set; }

        private string AccessToken { get; set; }

        private JwtService JwtService { get; set; }

        private HttpClientService HttpClientService { get; set; }

        public ProfileService(string apiBaseUrl, string accessToken, JwtService jwtService, HttpClientService httpClientService)
        {
            ApiBaseUrl = apiBaseUrl;
            AccessToken = accessToken;
            JwtService = jwtService;
            HttpClientService = httpClientService;
        }

        public async Task<Either<Exception, ProfileData>> GetProfileData(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken))
                    throw new ArgumentNullException(nameof(AccessToken));

                var endpoint = GetProfilePath;
                if (ApiBaseUrl.EndsWith("/"))
                    endpoint = endpoint.Remove(0, 1);

                if (string.IsNullOrEmpty(username))
                    throw new NullReferenceException("User name no fue encontrado en el AccessToken");

                endpoint = endpoint.Replace("{username}", username);

                var url = $"{ApiBaseUrl}{endpoint}";
                var eitherData = await HttpClientService.ExecuteGetAsync<ProfileData>(url);
                var data = eitherData.Map(d => d);

                return Either<Exception, ProfileData>.Right(data);
            }
            catch (Exception ex)
            {
                return Either<Exception, ProfileData>.Left(ex);
            }
        }

        public async Task<Either<Exception, PostsResponse>> GetProfilePosts(string username, int pageToLoad = 5)
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken))
                    throw new ArgumentNullException(nameof(AccessToken));

                var endpoint = GetProfilePostsPath;
                if (ApiBaseUrl.EndsWith("/"))
                    endpoint = endpoint.Remove(0, 1);

                if (string.IsNullOrEmpty(username))
                    throw new NullReferenceException("User name no fue encontrado en el AccessToken");

                endpoint = endpoint.Replace("{username}", username);
                endpoint = endpoint.Replace("{page}", pageToLoad.ToString());

                var eitherData = await HttpClientService.ExecuteGetAsync<PostsResponse>(endpoint);
                var data = eitherData.Map(d => d);

                return Either<Exception, PostsResponse>.Right(data);
            }
            catch (Exception ex)
            {
                return Either<Exception, PostsResponse>.Left(ex);
            }
        }

    }
}
