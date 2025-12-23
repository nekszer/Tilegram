using System.ComponentModel.DataAnnotations;

namespace Tilegram.Feature.Authentication
{
	public class AuthenticationRequest
	{
		[Required(ErrorMessage = "El usuario es requerido")]
		[Request("username")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "La contraseña es requerida")]
		[Request("password")]
		public string Password { get; set; }
	}
}
