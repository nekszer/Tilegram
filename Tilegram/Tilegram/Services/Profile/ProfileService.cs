using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tilegram.Services.Authentication;

namespace Tilegram.Services.Profile
{
    public class ProfileService
    {

        private const string EndPoint = "/api/Profile/{username}";

        private string ApiBaseUrl { get; set; }

        private string AccessToken { get; set; }

        private JwtService JwtService { get; set; }

        public ProfileService(string apiBaseUrl, string accessToken, JwtService jwtService)
        {
            ApiBaseUrl = apiBaseUrl;
            AccessToken = accessToken;
            JwtService = jwtService;
        }

        public async Task<Either<Exception, ProfileData>> Me()
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken))
                    throw new ArgumentNullException(nameof(AccessToken));

                var endpoint = EndPoint;
                if (ApiBaseUrl.EndsWith("/"))
                    endpoint = EndPoint.Remove(0, 1);

                var payLoad = JwtService.ReadPayload(AccessToken);
                var userName = payLoad["userName"]?.ToString() ?? string.Empty;

                if (string.IsNullOrEmpty(userName))
                    throw new NullReferenceException("User name no fue encontrado en el AccessToken");

                endpoint = endpoint.Replace("{username}", userName);
                

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accessToken", AccessToken);
                var response = await httpClient.GetAsync($"{ApiBaseUrl}{endpoint}");

                if (!response.IsSuccessStatusCode)
                    return Either<Exception, ProfileData>.Left(new InvalidOperationException("Error al obtener los datos del usuario"));

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProfileData>(json);
                return Either<Exception, ProfileData>.Right(data);
            }
            catch (Exception ex)
            {
                return Either<Exception, ProfileData>.Left(ex);
            }
        }

    }

    public class ProfileData
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("followingCount")]
        public int FollowingCount { get; set; }

        [JsonProperty("followerCount")]
        public int FollowerCount { get; set; }

        [JsonProperty("postCount")]
        public int PostCount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
