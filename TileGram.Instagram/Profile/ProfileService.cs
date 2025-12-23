using InstagramApiSharp;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Logger;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Tilegram.Feature.Profile
{
    public class ProfileService : IProfileService
    {
        private ISessionHandler SessionHandler { get; }
        public ProfileService(ISessionHandler sessionHandler)
        {
            SessionHandler = sessionHandler;
        }

        public async Task<Either<Exception, ProfileData>> GetProfileData(string username)
        {
            try
            {
                var _instaApi = InstaApiBuilder.CreateBuilder()
                    .UseLogger(new DebugLogger(LogLevel.Exceptions))
                    .Build();

                var instagramSessionData = await SessionHandler.GetSessionData();
                await _instaApi.LoadStateDataFromStringAsync(instagramSessionData.Json);
                var info = await _instaApi.UserProcessor.GetUserInfoByUsernameAsync(username);
                if (!(info?.Succeeded ?? false))
                    return Either<Exception, ProfileData>.Left(new ProfileNotFoundException());

                var value = info.Value;

                return Either<Exception, ProfileData>.Right(new ProfileData
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
                return Either<Exception, ProfileData>.Left(ex);
            }
        }

        public async Task<Either<Exception, PostsResponse>> GetProfilePosts(string username, int pageToLoad = 5)
        {
            try
            {
                var _instaApi = InstaApiBuilder.CreateBuilder()
                    .UseLogger(new DebugLogger(LogLevel.Exceptions))
                    .Build();

                var instagramSessionData = await SessionHandler.GetSessionData();
                await _instaApi.LoadStateDataFromStringAsync(instagramSessionData.Json);

                var info = await _instaApi.UserProcessor.GetUserInfoByUsernameAsync(username);
                if (!(info?.Succeeded ?? false))
                    return Either<Exception, PostsResponse>.Left(new ProfileNotFoundException());

                var userMedia = await _instaApi
                    .UserProcessor
                    .GetUserMediaAsync(username, PaginationParameters.MaxPagesToLoad(pageToLoad));

                if (!userMedia.Succeeded)
                    return Either<Exception, PostsResponse>.Left(new ProfileDataNotFoundException());

                // Mapear a un DTO más limpio
                var posts = userMedia.Value.Select(m => new PostsResponse.Post
                {
                    Id = m.InstaIdentifier,
                    Code = m.Code,
                    Text = m.Caption?.Text,
                    Images = m.Images.Select(s => s.Uri).ToList(),
                    LikesCount = m.LikesCount,
                    CommentsCount = m.CommentsCount,
                    TakenAt = m.TakenAt
                }).ToList();

                var userInfo = info.Value;
                float allMediaCount = userInfo.MediaCount;
                float mediaCountPage = posts.Count;
                var totalPages = 1f;

                if (allMediaCount > mediaCountPage)
                    totalPages = allMediaCount / mediaCountPage;

                var intTotalPages = (int)totalPages;
                float floatResidual = totalPages - intTotalPages;

                if (floatResidual > 0.0)
                    totalPages = intTotalPages + 1;

                return Either<Exception, PostsResponse>.Right (new PostsResponse
                {
                    CurrentPage = pageToLoad,
                    TotalPages = (int) totalPages,
                    TotalMedia = (int) allMediaCount,
                    ProcessMedia = mediaCountPage,
                    Posts = posts
                });
            }
            catch (Exception ex)
            {
                return Either<Exception, PostsResponse>.Left(ex);
            }
        }
    }

    public class ProfileNotFoundException : Exception
    {

    }

    public class ProfileDataNotFoundException : Exception
    {

    }
}
