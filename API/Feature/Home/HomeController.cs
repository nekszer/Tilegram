using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using Microsoft.AspNetCore.Mvc;
using Tilegram.Feature;

namespace Feature.Home
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IWebHostEnvironment Env { get; set; }
        private JwtService JwtService { get; set; }

        public HomeController(IWebHostEnvironment env, JwtService jwtService)
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

        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed([FromHeader] string accessToken)
        {
            var _instaApi = await LoadSession(accessToken);
            if (_instaApi == null)
                return Unauthorized();

            try
            {
                var userFeed = await _instaApi.FeedProcessor.GetUserTimelineFeedAsync(PaginationParameters.MaxPagesToLoad(5));
                if (!userFeed.Succeeded)
                    return NoContent();

                var dto = userFeed.Value.Medias.Select(s => InstagramMapper.MapToDto(s));
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }

    #region InstagramFeedDto

    public static class InstagramMapper
    {
        public static InstagramMediaDto MapToDto(InstaMedia mediaItem)
        {
            var dto = new InstagramMediaDto
            {
                Id = mediaItem.Pk,
                Code = mediaItem.Code,
                MediaType = (InstaMediaType)mediaItem.MediaType,
                Width = mediaItem.Width,
                Height = int.TryParse(mediaItem.Height?.ToString(), out int height) ? height : 0,
                TakenAt = mediaItem.TakenAt,
                LikesCount = mediaItem.LikesCount,
                CommentsCount = int.TryParse(mediaItem.CommentsCount?.ToString(), out int comments) ? comments : 0,
                HasLiked = mediaItem.HasLiked,
                HasSaved = mediaItem.HasViewerSaved,
                CanComment = !(mediaItem.IsCommentsDisabled),
                ViewCount = mediaItem.ViewCount,
                VideoDuration = mediaItem.VideoDuration,
                User = MapUser(mediaItem.User)
            };

            // Mapear caption
            dto.Caption = mediaItem.Caption?.Text?.ToString() ?? string.Empty;

            // Determinar tipo de contenido y URLs
            switch (dto.MediaType)
            {
                case InstaMediaType.Image:
                    dto.IsVideo = false;
                    dto.IsCarousel = false;
                    dto.MediaUrl = GetBestImageUrl(mediaItem.Images);
                    break;

                case InstaMediaType.Video:
                    dto.IsVideo = true;
                    dto.IsCarousel = false;
                    dto.MediaUrl = GetBestVideoUrl(mediaItem.Videos);
                    dto.ThumbnailUrl = GetBestImageUrl(mediaItem.Images);
                    break;

                case InstaMediaType.Carousel:
                    dto.IsVideo = false;
                    dto.IsCarousel = true;

                    // Para carousels, obtener la primera imagen como thumbnail
                    if (mediaItem.Carousel != null && mediaItem.Carousel.Count > 0)
                    {
                        var firstItem = mediaItem.Carousel[0];
                        dto.ThumbnailUrl = GetBestImageUrl(firstItem.Images);

                        // Obtener todas las URLs del carousel
                        foreach (var item in mediaItem.Carousel)
                        {
                            var url = GetBestImageUrl(item.Images);
                            if (!string.IsNullOrEmpty(url))
                                dto.CarouselUrls.Add(url);
                        }
                    }
                    else
                    {
                        dto.ThumbnailUrl = GetBestImageUrl(mediaItem.Images);
                    }
                    break;
            }

            return dto;
        }

        private static InstagramUserDto MapUser(InstaUser user)
        {
            if (user == null) return null;

            return new InstagramUserDto
            {
                Id = user.Pk,
                Username = user.UserName,
                FullName = user.FullName,
                ProfilePictureUrl = user.ProfilePicture ?? user.ProfilePicUrl,
                IsVerified = user.IsVerified,
                IsPrivate = user.IsPrivate,
                FollowersCount = user.FollowersCount
            };
        }

        private static string GetBestImageUrl(List<InstaImage> images)
        {
            if (images == null || images.Count == 0) 
                return string.Empty;

            var bestImage = images.OrderByDescending(img => img.Width * img.Height)
                                    .FirstOrDefault();

            return bestImage?.Uri ?? string.Empty;
        }

        private static string GetBestVideoUrl(List<InstaVideo> videos)
        {
            if (videos == null || videos.Count == 0) return string.Empty;

            var bestVideo = videos.FirstOrDefault(v => v.Type == 101)
                           ?? videos.FirstOrDefault();

            return bestVideo?.Uri ?? string.Empty;
        }
    }

    public class InstagramFeedDto
    {
        public int MediaItemsCount { get; set; }
        public string NextMaxId { get; set; }
        public List<InstagramMediaDto> Medias { get; set; } = new List<InstagramMediaDto>();
    }

    public class InstagramMediaDto
    {
        // Información básica
        public string Id { get; set; } // pk
        public string Code { get; set; } // para construir URLs: instagram.com/p/{Code}/
        public InstaMediaType MediaType { get; set; }

        // Contenido multimedia
        public string MediaUrl { get; set; } // URL principal (imagen o video)
        public string ThumbnailUrl { get; set; } // Thumbnail para videos o carousel
        public List<string> CarouselUrls { get; set; } = new List<string>(); // Para carousels
        public bool IsVideo { get; set; }
        public bool IsCarousel { get; set; }
        public double? VideoDuration { get; set; } // En segundos

        // Dimensiones
        public int Width { get; set; }
        public int Height { get; set; }
        public double AspectRatio => Height > 0 ? (double)Width / Height : 1.0;

        // Información del usuario
        public InstagramUserDto User { get; set; }

        // Metadatos de la publicación
        public DateTime TakenAt { get; set; }
        public string Caption { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }

        // Interacción del usuario actual
        public bool HasLiked { get; set; }
        public bool HasSaved { get; set; }
        public bool CanComment { get; set; } = true;

        // Estadísticas de visualización (para videos)
        public int? ViewCount { get; set; }

        // Propiedades calculadas para UI
        public string TimeAgo => GetTimeAgo(TakenAt);
        public string LikesCountFormatted => FormatCount(LikesCount);
        public string CommentsCountFormatted => FormatCount(CommentsCount);

        // Métodos auxiliares
        private string GetTimeAgo(DateTime dateTime)
        {
            var span = DateTime.UtcNow - dateTime;

            if (span.TotalDays > 365)
                return $"{(int)(span.TotalDays / 365)}y";
            if (span.TotalDays > 30)
                return $"{(int)(span.TotalDays / 30)}mo";
            if (span.TotalDays > 7)
                return $"{(int)(span.TotalDays / 7)}w";
            if (span.TotalDays >= 1)
                return $"{(int)span.TotalDays}d";
            if (span.TotalHours >= 1)
                return $"{(int)span.TotalHours}h";
            if (span.TotalMinutes >= 1)
                return $"{(int)span.TotalMinutes}m";

            return "Just now";
        }

        private string FormatCount(int count)
        {
            if (count >= 1000000)
                return $"{(count / 1000000.0):0.#}M";
            if (count >= 1000)
                return $"{(count / 1000.0):0.#}K";
            return count.ToString();
        }
    }

    public class InstagramUserDto
    {
        public long Id { get; set; } // pk
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsVerified { get; set; }
        public bool IsPrivate { get; set; }
        public int? FollowersCount { get; set; }

        // Propiedad calculada
        public string FollowersCountFormatted =>
            FollowersCount.HasValue ? FormatCount(FollowersCount.Value) : null;

        private string FormatCount(int count)
        {
            if (count >= 1000000)
                return $"{(count / 1000000.0):0.#}M";
            if (count >= 1000)
                return $"{(count / 1000.0):0.#}K";
            return count.ToString();
        }
    }

    public enum InstaMediaType
    {
        Image = 1,
        Video = 2,
        Carousel = 8
    }
    #endregion
}
