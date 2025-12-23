using Light.UWP.Services.IoC;
using Light.UWP.Services.Navigation;
using Light.UWP.Services.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tilegram.Feature.Authentication
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var connectionEither = await Connectivity.Current.IsConnected();
            connectionEither.Status(OnConnectivitySuccess, OnConnectivityError);
        }

        private async void OnConnectivitySuccess()
        {
            var authenticationService = Container.Instance.Resolve<AuthenticationService>();
            var result = await authenticationService.LogIn(new AuthenticationRequest
            {
                UserName = UsernameTextBox.Text,
                Password = PasswordBox.Password
            });
            result.Match(OnLogInError, OnLogInSuccess);
        }

        private async void OnConnectivityError(string obj)
        {
            MessageDialog messageDialog = new MessageDialog("Información");
            messageDialog.Content = "Revisa tu conexión a internet";
            await messageDialog.ShowAsync();
        }

        private void OnLogInSuccess(AuthenticationResponse success)
        {
            // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOjYyMjA0NDQ2NDcxLCJuYW1lIjoiU29mw61hIENhc3RpbGxvIiwidXNlck5hbWUiOiJiY2FzX3NvZmlhIiwicGljdHVyZSI6Imh0dHBzOi8vaW5zdGFncmFtLmZtZXgxMC00LmZuYS5mYmNkbi5uZXQvdi90NTEuMjg4NS0xOS81MzcyNTIyMTFfMTc5MjAwMjE2NTkwNzg0NzJfNzg3NjM3Njg5ODM5NzQxOTg3MV9uLmpwZz9zdHA9ZHN0LWpwZ19lMF9zMTUweDE1MF90dDYmZWZnPWV5SjJaVzVqYjJSbFgzUmhaeUk2SW5CeWIyWnBiR1ZmY0dsakxtUnFZVzVuYnk0eE1EZ3dMbU15SW4wJl9uY19odD1pbnN0YWdyYW0uZm1leDEwLTQuZm5hLmZiY2RuLm5ldCZfbmNfY2F0PTEwNCZfbmNfb2M9UTZjWjJRR25pWk5VVUdBMWdRd0s1NVgtSldDSktMTWdZN1BnNG9zOUE2SVNfZE1SWXpJQ3h0NmtvRk1HTWtTM3M3UW9ZN00mX25jX29oYz13QkV1a3dNdEV6MFE3a052d0habThIbyZlZG09QUFBQUFBQUJBQUFBJmNjYj03LTUmaWdfY2FjaGVfa2V5PUdIUFJCU0JJVDFzdEtxby1BRjg1RXVvT2cwNXRibU5EQVFBQjE1MDE1MDBqLWNjYjctNSZvaD0wMF9BZmxiUW1uNkpqdXBzZXAzcFJFOTRhUWxkQ2VhWTNZbXlTYzNRa0lLQlMxMHpnJm9lPTY5NEJCOTcyJl9uY19zaWQ9MzI4MjU5IiwiaWF0IjoxNzY2MTkzNzUxLCJleHAiOjE3Njg3ODU3NTF9.VxvzEAuLN2kUo3mcLffDuoz5TIdZa-KZjTRSwXjUhz4
            AppSettings.AccessToken = success.AccessToken;
            AppSettings.AccessTokenExpires = success.Expires;
            
            var userData = Container.Instance.Resolve<JwtService>().ReadPayload(success.AccessToken);

            if (userData.ContainsKey("picture"))
                AppSettings.UserPicture = userData["picture"]?.ToString() ?? string.Empty;

            if (userData.ContainsKey("name"))
                AppSettings.UserFullName = userData["name"]?.ToString() ?? string.Empty;

            if (userData.ContainsKey("userName"))
                AppSettings.UserName = userData["userName"]?.ToString() ?? string.Empty;

            NavigationService.Instance.NavigateTo(AppRoutes.Profile);
        }

        private async void OnLogInError(Exception exception)
        {
            var dialog = new MessageDialog("Info");

            if (exception is ValidationException validationException)
                dialog.Content = validationException.Message;
            else
                dialog.Content = "No es posible inciar sesión, revisa tus datos.";

            await dialog.ShowAsync();
        }
    }
}