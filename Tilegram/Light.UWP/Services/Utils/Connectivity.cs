
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Light.UWP.Services.Utils
{
    public class Connectivity
    {

        private static Connectivity _current;
        public static Connectivity Current
        {
            get
            {
                if (_current == null)
                    _current = new Connectivity();

                return _current;
            }
        }

        private Connectivity() { }

        public ConnectivityStatus InternetStatus
        {
            get
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();
                if (profile == null || profile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return ConnectivityStatus.Disconnected;

                return ConnectivityStatus.Connected;
            }
        }

        public async Task<ConnectivityEither> IsConnected()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    var response = await client.GetAsync("https://www.microsoft.com");
                    var status = response.IsSuccessStatusCode && InternetStatus == ConnectivityStatus.Connected;
                    return new ConnectivityEither(status, status ? string.Empty : "ErrNetwork");
                }
            }
            catch (Exception ex)
            {
                return new ConnectivityEither(false, ex.Message);
            }
        }
    }

    public class ConnectivityEither
    {
        public bool IsConnected { get; set; }
        public string ErrorMessage { get; set; }
        internal ConnectivityEither(bool isConnected, string errorMessage) { IsConnected = isConnected; ErrorMessage = errorMessage;  }

        public void Status(Action connected, Action<string> disconnected)
        {
            if (IsConnected)
                connected?.Invoke();
            else
                disconnected?.Invoke(ErrorMessage);
        }

        public T CheckStatus<T>(Func<T> connected, Func<string, T> disconnected)
        {
            if (IsConnected)
                return connected.Invoke();
            else
                return disconnected.Invoke(ErrorMessage);
        }
    }

    public enum ConnectivityStatus
    {
        Disconnected = 0, Connected = 1
    }
}