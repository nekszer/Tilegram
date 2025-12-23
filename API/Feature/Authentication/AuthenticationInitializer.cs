namespace Tilegram.Feature.Authentication
{
    public static class AuthenticationInitializer
    {
        public static void AddAuthenticationFeature(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

    }
}