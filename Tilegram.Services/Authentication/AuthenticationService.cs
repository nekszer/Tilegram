using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Tilegram.Feature.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private string apiBaseUrl;

        private HttpClientService HttpClientService { get; set; }

        public AuthenticationService(string apiBaseUrl, HttpClientService httpClientService)
        {
            this.apiBaseUrl = apiBaseUrl;
            HttpClientService = httpClientService;
        }

        public async Task<Either<Exception, AuthenticationResponse>> LogIn(AuthenticationRequest request)
        {
            try
            {
                var endpoint = AuthenticationConstants.LogInEndPoint;
                if (apiBaseUrl.EndsWith("/"))
                    endpoint = endpoint.Remove(0, 1);

                Validator.ValidateObject(request, new ValidationContext(request));

                var url = $"{apiBaseUrl}{endpoint}";
                var eitherResponse = await HttpClientService
                    .FormUrlEncodedContent(request)
                    .ExecutePostAsync<AuthenticationResponse>(url);

                var data = eitherResponse.Map(d => d);
                return Either<Exception, AuthenticationResponse>.Right(data);
            }
            catch (Exception ex)
            {
                return Either<Exception, AuthenticationResponse>.Left(ex);
            }
        }

    }

}
