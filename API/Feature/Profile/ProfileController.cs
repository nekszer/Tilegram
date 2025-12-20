using API.Feature.Profile;
using API.Services;
using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;
using Microsoft.AspNetCore.Mvc;

namespace Feature.Profile
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private IWebHostEnvironment Env { get; set; }
        private JwtService JwtService { get; set; }

        public ProfileController(IWebHostEnvironment env, JwtService jwtService)
        {
            Env = env;
            JwtService = jwtService;
        }

        private async Task<IInstaApi> LoadSession(string accessToken)
        {
            try
            {
                var _instaApi = InstaApiBuilder.CreateBuilder()
                    .UseLogger(new DebugLogger(InstagramApiSharp.Logger.LogLevel.Exceptions))
            .Build();

                var payload = JwtService.ReadPayload(accessToken);
                var userId = payload["sub"];

                var path = Path.Combine(Env.ContentRootPath, @"InstaSessions\" + $"{userId}.json");
                var jsonSession = await System.IO.File.ReadAllTextAsync(path);

                await _instaApi.LoadStateDataFromStringAsync(jsonSession);

                if (!_instaApi.IsUserAuthenticated)
                    return null;

                return _instaApi;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile([FromHeader] string accessToken, string username)
        {
            var _instaApi = await LoadSession(accessToken);
            if (_instaApi == null)
                return Unauthorized();

            try
            {
                var user = await _instaApi.UserProcessor.GetUserAsync(username);
                if (!(user?.Succeeded ?? false))
                    return NotFound(user?.Info?.Message ?? "No se encontro el usuario");

                var info = await _instaApi.UserProcessor.GetUserInfoByIdAsync(user.Value.Pk);
                if (!(info?.Succeeded ?? false))
                    return NotFound();

                var value = info.Value;

                return Ok(new ProfileInfo
                {
                    FullName = value.FullName,
                    UserName = value.Username,
                    Picture = value.ProfilePicUrl,
                    FollowerCount = value.FollowerCount,
                    FollowingCount = value.FollowingCount,
                    PostCount = value.MediaCount,
                    Description = value.Biography
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("posts/{username}")]
        public async Task<IActionResult> GetUserPosts([FromHeader] string accessToken, string username, int page = 1)
        {
            var _instaApi = await LoadSession(accessToken);
            if (_instaApi == null)
                return Unauthorized();

            try
            {
                var userMedia = await _instaApi
                .UserProcessor
                .GetUserMediaAsync(username, PaginationParameters.MaxPagesToLoad(page));

                if (!userMedia.Succeeded)
                    return BadRequest(userMedia.Info?.Message);

                // Mapear a un DTO más limpio
                var posts = userMedia.Value.Select(m => new UserMedia
                {
                    Id = m.InstaIdentifier,
                    Code = m.Code,
                    Text = m.Caption?.Text,
                    Images = m.Images.Select(s => s.Uri),
                    LikesCount = m.LikesCount,
                    CommentsCount = m.CommentsCount,
                    TakenAt = m.TakenAt
                });

                return Ok(new UserMedia
                {

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
