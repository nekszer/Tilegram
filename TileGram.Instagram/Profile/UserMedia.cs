using System;
using System.Collections.Generic;

namespace Tilegram.Feature.Profile
{
    internal class UserMedia
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }
        public IEnumerable<string> Images { get; set; }
        public int LikesCount { get; set; }
        public string CommentsCount { get; set; }
        public DateTime TakenAt { get; set; }
    }
}