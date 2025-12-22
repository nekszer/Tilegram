using Light.UWP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tilegram.Services.Authentication
{
    public class AuthenticationResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("expires")]
        public long Expires { get; set; }
    }

    public class AuthenticationRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AuthenticationService
    {
        private const string EndPoint = "/api/authentication/login";

        private string apiBaseUrl;

        public AuthenticationService(string apiBaseUrl)
        {
            this.apiBaseUrl = apiBaseUrl;
        }

        public async Task<Either<Exception, AuthenticationResponse>> LogIn(AuthenticationRequest request)
        {
            try
            {
                var endpoint = EndPoint;
                if(apiBaseUrl.EndsWith("/"))
                    endpoint = EndPoint.Remove(0, 1);

                Validator.ValidateObject(request, new ValidationContext(request));

                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync($"{apiBaseUrl}{endpoint}", new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", request.UserName),
                    new KeyValuePair<string, string>("password", request.Password)
                }));

                if (!response.IsSuccessStatusCode)
                    return Either<Exception, AuthenticationResponse>.Left(new InvalidOperationException("Error al iniciar sesión"));

                var json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<AuthenticationResponse>(json);
                return Either<Exception, AuthenticationResponse>.Right(data);
            }
            catch (Exception ex)
            {
                return Either<Exception, AuthenticationResponse>.Left(ex);
            }
        }

    }

    
}
