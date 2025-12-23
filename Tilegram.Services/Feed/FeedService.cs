using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tilegram.Feature.Feed
{
    public class FeedService : IFeedService
    {

        private const string GetFeed = "/api/Home/feed";

        private string ApiBaseUrl { get; set; }

        private string AccessToken { get; set; }

        private JwtService JwtService { get; set; }
        private HttpClientService HttpClientService { get; set; }

        public FeedService(string apiBaseUrl, string accessToken, JwtService jwtService, HttpClientService httpClientService)
        {
            ApiBaseUrl = apiBaseUrl;
            AccessToken = accessToken;
            JwtService = jwtService;
            HttpClientService = httpClientService;
        }

        public async Task<Either<Exception, List<FeedItem>>> UserFeed()
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken))
                    throw new ArgumentNullException(nameof(AccessToken));

                var endpoint = GetFeed;
                if (ApiBaseUrl.EndsWith("/"))
                    endpoint = endpoint.Remove(0, 1);

                var url = $"{ApiBaseUrl}{endpoint}";

                var eitherData = await HttpClientService.ExecuteGetAsync<List<FeedItem>>(url);
                var data = eitherData.Map(d => d);
                return Either<Exception, List<FeedItem>>.Right(data);
            }
            catch (Exception ex)
            {
                return Either<Exception, List<FeedItem>>.Left(ex);
            }
        }
    }
}
