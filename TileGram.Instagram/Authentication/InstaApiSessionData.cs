using System.Collections.Generic;

namespace Tilegram.Feature.Authentication
{
    public class InstaApiSessionData
    {
        public DeviceInfoData DeviceInfo { get; set; }
        public UserSessionData UserSession { get; set; }
        public bool IsAuthenticated { get; set; }
        public CookiesData Cookies { get; set; }
        public List<object> RawCookies { get; set; }
        public int InstaApiVersion { get; set; }
        public object TwoFactorLoginInfo { get; set; }
        public object ChallengeLoginInfo { get; set; }
        public object ChallengeVerifyMethod { get; set; }

        public class AndroidVer
        {
            public string Codename { get; set; }
            public string VersionNumber { get; set; }
            public string APILevel { get; set; }
        }

        public class CookiesData
        {
            public int Capacity { get; set; }
            public int Count { get; set; }
            public int MaxCookieSize { get; set; }
            public int PerDomainCapacity { get; set; }
        }

        public class DeviceInfoData
        {
            public string PhoneGuid { get; set; }
            public string DeviceGuid { get; set; }
            public string GoogleAdId { get; set; }
            public string RankToken { get; set; }
            public string AdId { get; set; }
            public string PigeonSessionId { get; set; }
            public string PushDeviceGuid { get; set; }
            public string FamilyDeviceGuid { get; set; }
            public AndroidVer AndroidVer { get; set; }
            public string AndroidBoardName { get; set; }
            public string AndroidBootloader { get; set; }
            public string DeviceBrand { get; set; }
            public string DeviceId { get; set; }
            public string DeviceModel { get; set; }
            public string DeviceModelBoot { get; set; }
            public string DeviceModelIdentifier { get; set; }
            public string FirmwareBrand { get; set; }
            public string FirmwareFingerprint { get; set; }
            public string FirmwareTags { get; set; }
            public string FirmwareType { get; set; }
            public string HardwareManufacturer { get; set; }
            public string HardwareModel { get; set; }
            public string Resolution { get; set; }
            public string Dpi { get; set; }
            public string IGBandwidthSpeedKbps { get; set; }
            public string IGBandwidthTotalBytesB { get; set; }
            public string IGBandwidthTotalTimeMS { get; set; }
        }

        public class LoggedInUser
        {
            public bool IsVerified { get; set; }
            public bool IsPrivate { get; set; }
            public long Pk { get; set; }
            public string ProfilePicture { get; set; }
            public string ProfilePicUrl { get; set; }
            public string ProfilePictureId { get; set; }
            public string UserName { get; set; }
            public string FullName { get; set; }
        }

        public class UserSessionData
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string PublicKey { get; set; }
            public string PublicKeyId { get; set; }
            public string WwwClaim { get; set; }
            public object FbTripId { get; set; }
            public string Authorization { get; set; }
            public string XMidHeader { get; set; }
            public string RurHeader { get; set; }
            public object RespondUDirectRegionHint { get; set; }
            public object RespondUShbid { get; set; }
            public object RespondUShbts { get; set; }
            public string RespondURur { get; set; }
            public object SessionFlushNonce { get; set; }
            public LoggedInUser LoggedInUser { get; set; }
            public string RankToken { get; set; }
            public object CsrfToken { get; set; }
            public string FacebookUserId { get; set; }
            public string FacebookAccessToken { get; set; }
        }
    }
}