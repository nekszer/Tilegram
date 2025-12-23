using System.ComponentModel.DataAnnotations;
using Tilegram.Feature.Authentication;

namespace Tilegram.Feature.Test.Authentication
{
    public class AuthenticationServiceTests
    {
        [Fact]
        public async Task LogIn_ReturnsRight_WhenCredentialsAreValid()
        {
            // Arrange
            var service = new AuthenticationService();
            var request = new AuthenticationRequest
            {
                UserName = "use_user",
                Password = "use_password"
            };

            // Act
            var result = await service.LogIn(request);

            // Assert
            Assert.True(result.Success, "El resultado debe ser Right cuando las credenciales son correctas");
            Assert.NotNull(result.RightValue);
            Assert.False(string.IsNullOrEmpty(result.RightValue.AccessToken));
        }

        [Fact]
        public async Task LogIn_Request_Validation_HasData()
        {
            // Arrange
            var request = new AuthenticationRequest
            {
                UserName = "hasData",
                Password = "hasData"
            };

            ValidationContext validationContext = new ValidationContext(request);
            Validator.ValidateObject(request, validationContext);
        }

        [Fact]
        public async Task LogIn_Request_Validation_EmptyData()
        {
            try
            {
                var request = new AuthenticationRequest();
                ValidationContext validationContext = new ValidationContext(request);
                Validator.ValidateObject(request, validationContext);
            }
            catch (Exception ex)
            {
                Assert.IsType<ValidationException>(ex);
            }
        }

        [Fact]
        public async Task LogIn_ReturnsLeft_WhenCredentialsAreInvalid()
        {
            // Arrange
            var service = new AuthenticationService();
            var request = new AuthenticationRequest
            {
                UserName = "usuario_invalido",
                Password = "password_invalido"
            };

            // Act
            var result = await service.LogIn(request);

            // Assert
            Assert.True(!result.Success, "El resultado debe ser Left cuando las credenciales son incorrectas");
            Assert.NotNull(result.LeftValue);
            Assert.IsType<InvalidCredentialsException>(result.LeftValue);
        }
    }

}
