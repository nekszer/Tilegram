
using Newtonsoft.Json;
using Tilegram.Feature.Authentication;

namespace Tilegram.Feature
{
    public class SessionHandler : ISessionHandler
    {
        private readonly IWebHostEnvironment _env;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHandler(IWebHostEnvironment env, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SessionData> GetSessionData()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var authHeader = httpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                throw new InvalidOperationException("No se encontró el access token en los headers");

            var accessToken = authHeader.Substring("Bearer ".Length);

            var payload = _jwtService.ReadPayload(accessToken);
            var userId = payload["sub"]?.ToString() ?? string.Empty;
            var expires = long.Parse(payload["exp"]?.ToString() ?? "0");

            var path = Path.Combine(_env.ContentRootPath, "InstaSessions", $"{userId}.json");
            var jsonSession = await File.ReadAllTextAsync(path);

            return new SessionData { Json = jsonSession, AccessToken = accessToken, Expires = expires };
        }

        public async Task<SessionData> SetSessionData(string json)
        {
            var data = JsonConvert.DeserializeObject<InstaApiSessionData>(json);

            var expires = DateTimeOffset.UtcNow.AddDays(30).ToUnixTimeSeconds();
            var token = _jwtService.GenerateToken(new Dictionary<string, object>
            {
                { "sub", data.UserSession.LoggedInUser.Pk },
                { "name",  data.UserSession.LoggedInUser.FullName },
                { "userName", data.UserSession.UserName },
                { "picture", data.UserSession.LoggedInUser.ProfilePicUrl },
                { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                { "exp", expires }
            });

            var path = Path.Combine(_env.ContentRootPath, @"InstaSessions\" + $"{data.UserSession.LoggedInUser.Pk}.json");
            await System.IO.File.WriteAllTextAsync(path, json);

            return new SessionData
            {
                Json = json,
                AccessToken = token,
                Expires = expires
            };
        }
    }

}
