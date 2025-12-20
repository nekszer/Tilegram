

namespace Feature.Profile
{
    internal class UserMedia
    {
        public string Id { get; internal set; }
        public string Code { get; internal set; }
        public string? Text { get; internal set; }
        public IEnumerable<string> Images { get; internal set; }
        public int LikesCount { get; internal set; }
        public string CommentsCount { get; internal set; }
        public DateTime TakenAt { get; internal set; }
    }
}