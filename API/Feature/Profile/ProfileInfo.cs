namespace API.Feature.Profile
{
    public class ProfileInfo
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
        public long FollowingCount { get; set; }
        public long FollowerCount { get; set; }
        public long PostCount { get; set; }
        public string Description { get; set; }
    }
}