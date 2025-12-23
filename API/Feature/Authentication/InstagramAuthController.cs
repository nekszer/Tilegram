using Microsoft.AspNetCore.Mvc;

namespace Tilegram.Feature.Authentication
{
    [ApiController]
    [Route("api/authentication")]
    public class InstagramAuthController : ControllerBase
    {
        private IAuthenticationService AuthenticationService { get; }
        private ISessionHandler SessionHandler { get; }

        public InstagramAuthController(IAuthenticationService authenticationService, ISessionHandler sessionHandler)
        {
            AuthenticationService = authenticationService;
            SessionHandler = sessionHandler;
        }

        [HttpPost(AuthenticationConstants.LogInEndPoint)]
        public async Task<IActionResult> Login([FromForm] AuthenticationRequest authenticationRequest)
        {
            try
            {
                var eitherResponse = await AuthenticationService.LogIn(authenticationRequest);
                var response = eitherResponse.Match(ex =>
                {
                    throw ex;
                }, async response =>
                {
                    var sessionData = await SessionHandler.SetSessionData(response.AccessToken);
                    response.AccessToken = sessionData.AccessToken;
                    response.Expires = sessionData.Expires;
                    return response;
                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                if(ex is InvalidCredentialsException invalidCredentialsException)
                    return Unauthorized(invalidCredentialsException.Message);

                return StatusCode(500, ex.Message);
            }
        }
    }
}