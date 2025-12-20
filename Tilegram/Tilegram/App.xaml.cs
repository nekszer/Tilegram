using Light.UWP.Services.IoC;
using Light.UWP.Services.Navigation;
using System;
using Tilegram.Feature.Authentication;
using Tilegram.Feature.Profile;
using Tilegram.Services.Authentication;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Tilegram
{
    /// <summary>
    /// Proporciona un comportamiento específico de la aplicación para complementar la clase Application predeterminada.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Inicializa el objeto de aplicación Singleton. Esta es la primera línea de código creado
        /// ejecutado y, como tal, es el equivalente lógico de main() o WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            RegisterServices();
            RegisterRoutes();
        }

        private void RegisterServices()
        {
            var container = Container.Instance;

            ApplicationData.Current.LocalSettings.Values["ApiBaseUrl"] = "http://localhost:5162/";
            ApplicationData.Current.LocalSettings.Values["AccessToken"] = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOjYyMjA0NDQ2NDcxLCJuYW1lIjoiU29mw61hIENhc3RpbGxvIiwidXNlck5hbWUiOiJiY2FzX3NvZmlhIiwicGljdHVyZSI6Imh0dHBzOi8vaW5zdGFncmFtLmZtZXgxMC00LmZuYS5mYmNkbi5uZXQvdi90NTEuMjg4NS0xOS81MzcyNTIyMTFfMTc5MjAwMjE2NTkwNzg0NzJfNzg3NjM3Njg5ODM5NzQxOTg3MV9uLmpwZz9zdHA9ZHN0LWpwZ19lMF9zMTUweDE1MF90dDYmZWZnPWV5SjJaVzVqYjJSbFgzUmhaeUk2SW5CeWIyWnBiR1ZmY0dsakxtUnFZVzVuYnk0eE1EZ3dMbU15SW4wJl9uY19odD1pbnN0YWdyYW0uZm1leDEwLTQuZm5hLmZiY2RuLm5ldCZfbmNfY2F0PTEwNCZfbmNfb2M9UTZjWjJRR25pWk5VVUdBMWdRd0s1NVgtSldDSktMTWdZN1BnNG9zOUE2SVNfZE1SWXpJQ3h0NmtvRk1HTWtTM3M3UW9ZN00mX25jX29oYz13QkV1a3dNdEV6MFE3a052d0habThIbyZlZG09QUFBQUFBQUJBQUFBJmNjYj03LTUmaWdfY2FjaGVfa2V5PUdIUFJCU0JJVDFzdEtxby1BRjg1RXVvT2cwNXRibU5EQVFBQjE1MDE1MDBqLWNjYjctNSZvaD0wMF9BZmxiUW1uNkpqdXBzZXAzcFJFOTRhUWxkQ2VhWTNZbXlTYzNRa0lLQlMxMHpnJm9lPTY5NEJCOTcyJl9uY19zaWQ9MzI4MjU5IiwiaWF0IjoxNzY2MTkzNzUxLCJleHAiOjE3Njg3ODU3NTF9.VxvzEAuLN2kUo3mcLffDuoz5TIdZa-KZjTRSwXjUhz4";

            container.Register(ioc => 
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                var apiBaseUrl = localSettings.Values["ApiBaseUrl"] as string;
                return new Services.Authentication.AuthenticationService(apiBaseUrl);
            });

            container.Register(ioc =>
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                var apiBaseUrl = localSettings.Values["ApiBaseUrl"] as string;
                var accessToken = localSettings.Values["AccessToken"] as string;
                return new Services.Profile.ProfileService(apiBaseUrl, accessToken, ioc.Resolve<JwtService>());
            });

            var jwtKey = "304e8693f44b4b0fcb17fd256187d65fdb16df8c9adc12338e4693e68438ca9b";
            container.Register(ioc => new JwtService(jwtKey));
        }

        private void RegisterRoutes()
        {
            var hasAccessToken = ApplicationData.Current.LocalSettings.Values["AccessToken"] as string;
            var needAuth = string.IsNullOrEmpty(hasAccessToken);

            var routeService = RouteService.Current;
            routeService.Add<LoginPage>("/auth", needAuth);
            routeService.Add<ProfilePage>("/profile", !needAuth);
        }

        #region App
        /// <summary>
        /// Se invoca cuando el usuario final inicia la aplicación normalmente. Se usarán otros puntos
        /// de entrada cuando la aplicación se inicie para abrir un archivo específico, por ejemplo.
        /// </summary>
        /// <param name="e">Información detallada acerca de la solicitud y el proceso de inicio.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // No repetir la inicialización de la aplicación si la ventana tiene contenido todavía,
            // solo asegurarse de que la ventana está activa.
            if (rootFrame == null)
            {
                // Crear un marco para que actúe como contexto de navegación y navegar a la primera página.
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Cargar el estado de la aplicación suspendida previamente
                }

                // Poner el marco en la ventana actual.
                Window.Current.Content = rootFrame;
            }

            var currentMainPage = NavigationService.Initialize(rootFrame);

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Cuando no se restaura la pila de navegación, navegar a la primera página,
                    // configurando la nueva página pasándole la información requerida como
                    //parámetro de navegación
                    rootFrame.Navigate(currentMainPage, e.Arguments);
                }
                // Asegurarse de que la ventana actual está activa.
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Se invoca cuando la aplicación la inicia normalmente el usuario final. Se usarán otros puntos
        /// </summary>
        /// <param name="sender">Marco que produjo el error de navegación</param>
        /// <param name="e">Detalles sobre el error de navegación</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Se invoca al suspender la ejecución de la aplicación. El estado de la aplicación se guarda
        /// sin saber si la aplicación se terminará o se reanudará con el contenido
        /// de la memoria aún intacto.
        /// </summary>
        /// <param name="sender">Origen de la solicitud de suspensión.</param>
        /// <param name="e">Detalles sobre la solicitud de suspensión.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Guardar el estado de la aplicación y detener toda actividad en segundo plano
            deferral.Complete();
        }
        #endregion
    }
}
