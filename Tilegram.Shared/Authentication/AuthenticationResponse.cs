using Newtonsoft.Json;

namespace Tilegram.Feature.Authentication
{
	public class AuthenticationResponse
	{
		[JsonProperty("accessToken")]
		public string AccessToken { get; set; }

		[JsonProperty("expires")]
		public long Expires { get; set; }
    }
}
