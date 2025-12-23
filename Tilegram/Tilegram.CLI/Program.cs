using System;
using Tilegram.Feature;
using Tilegram.Feature.Authentication;

namespace Tilegram.CLI
{
	class Program
	{
		static void Main(string[] args)
		{
			var authenticationService = new AuthenticationService("http://localhost:5162/", new HttpClientService(string.Empty));
			authenticationService.LogIn(new AuthenticationRequest
			{
				UserName = "nekszer",
				Password = "NXan0104"
			}).ContinueWith(t =>
			{
				var r = t.Result;
				r.Match(AuthenticationError, AuthenticationSuccess);
			});

			Console.ReadKey();
		}

		public static void AuthenticationError(Exception ex)
		{
			Console.WriteLine(ex.Message, "Authentication");
		}

		public static void AuthenticationSuccess(AuthenticationResponse response)
		{
			Console.WriteLine(response.AccessToken, "Authentication");
		}
	}
}
