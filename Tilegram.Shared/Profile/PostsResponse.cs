using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Tilegram.Feature.Profile
{
	public class PostsResponse
	{
		[JsonProperty("currentPage")]
		public int CurrentPage { get; set; }

		[JsonProperty("totalMedia")]
		public int TotalMedia { get; set; }

		[JsonProperty("totalPages")]
		public int TotalPages { get; set; }

		[JsonProperty("posts")]
		public List<Post> Posts { get; set; }

		[JsonProperty("processMedia")]
        public float ProcessMedia { get; set; }

        public class Post
		{
			[JsonProperty("id")]
			public string Id { get; set; }

			[JsonProperty("code")]
			public string Code { get; set; }

			[JsonProperty("text")]
			public string Text { get; set; }

			[JsonProperty("images")]
			public List<string> Images { get; set; }

			[JsonProperty("likesCount")]
			public int LikesCount { get; set; }

			[JsonProperty("commentsCount")]
			public string CommentsCount { get; set; }

			[JsonProperty("takenAt")]
			public DateTime TakenAt { get; set; }
		}
	}
}
