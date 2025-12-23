using System;
using System.Collections.Generic;

namespace Tilegram.Feature
{
    public interface IJwtService
    {
        string GenerateToken(Dictionary<string, object> payload, TimeSpan? expiry = null);
        IDictionary<string, object> ReadPayload(string token);
    }
}