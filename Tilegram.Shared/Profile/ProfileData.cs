using Newtonsoft.Json;

namespace Tilegram.Feature.Profile
{

	public class ProfileData
	{
		[JsonProperty("fullName")]
		public string FullName { get; set; }

		[JsonProperty("userName")]
		public string UserName { get; set; }

		[JsonProperty("picture")]
		public string Picture { get; set; }

		[JsonProperty("followingCount")]
		public long FollowingCount { get; set; }

		[JsonProperty("followerCount")]
		public long FollowerCount { get; set; }

		[JsonProperty("postCount")]
		public long PostCount { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }
	}
}