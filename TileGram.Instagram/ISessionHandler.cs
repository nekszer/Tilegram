using System.Threading.Tasks;

namespace Tilegram.Feature
{
    public interface ISessionHandler
    {
        Task<SessionData> GetSessionData();
        Task<SessionData> SetSessionData(string json);
    }

    public class SessionData
    {
        public string Json { get; set; }
        public string AccessToken { get; set; }
        public long Expires { get; set; }
    }
}