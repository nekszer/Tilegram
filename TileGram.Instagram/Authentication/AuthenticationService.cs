
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using System;
using System.Threading.Tasks;

namespace Tilegram.Feature.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<Either<Exception, AuthenticationResponse>> LogIn(AuthenticationRequest request)
        {
            var userSession = new UserSessionData
            {
                UserName = request.UserName,
                Password = request.Password
            };

            var _instaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(userSession)
                    .UseLogger(new DebugLogger(InstagramApiSharp.Logger.LogLevel.Exceptions))
                    .Build();

            if (!_instaApi.IsUserAuthenticated)
            {
                Console.WriteLine($"Logging in as {userSession.UserName}");
                var logInResult = await _instaApi.LoginAsync();

                if (!logInResult.Succeeded)
                {
                    Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                    return Either<Exception, AuthenticationResponse>.Left(new InvalidCredentialsException(logInResult.Info.Message));
                }

                Console.WriteLine($"Success login: {logInResult.Info.Message}");
            }

            var json = await _instaApi.GetStateDataAsStringAsync();

            return Either<Exception, AuthenticationResponse>.Right(new AuthenticationResponse
            {
                AccessToken = json
            });
        }
    }

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message) : base(message) { }
    }
}
