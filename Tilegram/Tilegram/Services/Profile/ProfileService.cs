using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tilegram.Services.Authentication;

namespace Tilegram.Services.Profile
{
    public class ProfileService
    {

        private const string GetProfilePath = "/api/Profile/{username}";
        private const string GetProfilePosts = "/api/Profile/posts/{username}?page={page}";

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

                var endpoint = GetProfilePath;
                if (ApiBaseUrl.EndsWith("/"))
                    endpoint = GetProfilePath.Remove(0, 1);

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

        public async Task<Either<Exception, PostsResponse>> MePosts(int pageToLoad = 5)
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken))
                    throw new ArgumentNullException(nameof(AccessToken));

                var endpoint = GetProfilePosts;
                if (ApiBaseUrl.EndsWith("/"))
                    endpoint = GetProfilePosts.Remove(0, 1);

                var payLoad = JwtService.ReadPayload(AccessToken);
                var userName = payLoad["userName"]?.ToString() ?? string.Empty;

                if (string.IsNullOrEmpty(userName))
                    throw new NullReferenceException("User name no fue encontrado en el AccessToken");

                endpoint = endpoint.Replace("{username}", userName);
                endpoint = endpoint.Replace("{page}", pageToLoad.ToString());

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accessToken", AccessToken);
                var response = await httpClient.GetAsync($"{ApiBaseUrl}{endpoint}");

                if (!response.IsSuccessStatusCode)
                    return Either<Exception, PostsResponse>.Left(new InvalidOperationException("Error al obtener los datos del usuario"));

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<PostsResponse>(json);
                return Either<Exception, PostsResponse>.Right(data);
            }
            catch (Exception ex)
            {
                return Either<Exception, PostsResponse>.Left(ex);
            }
        }

    }

    public class PostsResponse
    {
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("totalMedia")]
        public int TotalMedia { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("posts")]
        public List<Post> Posts { get; set; }

        public class Post
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("images")]
            public List<string> Images { get; set; }

            [JsonProperty("likesCount")]
            public int LikesCount { get; set; }

            [JsonProperty("commentsCount")]
            public string CommentsCount { get; set; }

            [JsonProperty("takenAt")]
            public DateTime TakenAt { get; set; }
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
