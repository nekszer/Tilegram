using Microsoft.AspNetCore.Mvc;
using Tilegram.Feature.Profile;

namespace Feature.Profile
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private IProfileService ProfileService { get; }

        public ProfileController(IProfileService profileService)
        {
            ProfileService = profileService;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile([FromHeader] string accessToken, string username)
        {
            try
            {
                var meEither = await ProfileService.GetProfileData(username);
                var profileData = meEither.Map(d => d);
                return Ok(profileData);
            }
            catch (Exception ex)
            {
                if(ex is ProfileNotFoundException)
                    return NotFound("Perfil no encontrado"); 

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("posts/{username}")]
        public async Task<IActionResult> GetUserPosts([FromHeader(Name = "Authorization")] string authorization, string username, int page = 5)
        {
            try
            {
                var profilePost = await ProfileService.GetProfilePosts(username, page);
                var posts = profilePost.Map(p => p);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                if (ex is ProfileNotFoundException)
                    return NotFound("Perfil no encontrado");

                if (ex is ProfileDataNotFoundException)
                    return NotFound("Error al extraer los post del usuario");

                return StatusCode(500, ex.Message);
            }
        }

    }
}
