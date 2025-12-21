using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tilegram.Feature.Feed
{
    public class FeedModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("mediaType")]
        public int MediaType { get; set; }

        [JsonProperty("mediaUrl")]
        public string MediaUrl { get; set; }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("carouselUrls")]
        public List<string> CarouselUrls { get; set; }

        [JsonProperty("isVideo")]
        public bool IsVideo { get; set; }

        [JsonProperty("isCarousel")]
        public bool IsCarousel { get; set; }

        [JsonProperty("videoDuration")]
        public double VideoDuration { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("aspectRatio")]
        public double AspectRatio { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("takenAt")]
        public DateTime TakenAt { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("likesCount")]
        public int LikesCount { get; set; }

        [JsonProperty("commentsCount")]
        public int CommentsCount { get; set; }

        [JsonProperty("hasLiked")]
        public bool HasLiked { get; set; }

        [JsonProperty("hasSaved")]
        public bool HasSaved { get; set; }

        [JsonProperty("canComment")]
        public bool CanComment { get; set; }

        [JsonProperty("viewCount")]
        public int ViewCount { get; set; }

        [JsonProperty("timeAgo")]
        public string TimeAgo { get; set; }

        [JsonProperty("likesCountFormatted")]
        public string LikesCountFormatted { get; set; }

        [JsonProperty("commentsCountFormatted")]
        public string CommentsCountFormatted { get; set; }
    }

    public class User
    {
        [JsonProperty("id")]
        public object Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("profilePictureUrl")]
        public string ProfilePictureUrl { get; set; }

        [JsonProperty("isVerified")]
        public bool IsVerified { get; set; }

        [JsonProperty("isPrivate")]
        public bool IsPrivate { get; set; }

        [JsonProperty("followersCount")]
        public int FollowersCount { get; set; }

        [JsonProperty("followersCountFormatted")]
        public string FollowersCountFormatted { get; set; }
    }
}
