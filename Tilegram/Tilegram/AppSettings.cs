using Windows.Storage;

namespace Tilegram
{
    public class AppSettings
    {

        public static string AccessToken
        {
            get => ApplicationData.Current.LocalSettings.Values["AccessToken"]?.ToString() ?? string.Empty;
            set => ApplicationData.Current.LocalSettings.Values["AccessToken"] = value;
        }

        public static long AccessTokenExpires
        {
            get => long.Parse(ApplicationData.Current.LocalSettings.Values["AccessTokenExpires"]?.ToString() ?? "0");
            set => ApplicationData.Current.LocalSettings.Values["AccessTokenExpires"] = value;
        }

        public static string UserPicture
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(UserPicture)]?.ToString() ?? string.Empty;
            set => ApplicationData.Current.LocalSettings.Values[nameof(UserPicture)] = value;
        }

        public static string UserName
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(UserName)]?.ToString() ?? string.Empty;
            set => ApplicationData.Current.LocalSettings.Values[nameof(UserName)] = value;
        }

        public static string UserFullName
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(UserFullName)]?.ToString() ?? string.Empty;
            set => ApplicationData.Current.LocalSettings.Values[nameof(UserFullName)] = value;
        }

        public static string ApiBaseUrl
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(ApiBaseUrl)]?.ToString() ?? string.Empty;
            set => ApplicationData.Current.LocalSettings.Values[nameof(ApiBaseUrl)] = value;
        }
    }
}