using System;

namespace Remosys.Common.Options
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public TimeSpan TokenLifeTime { get; set; }

        public string ValidIssuer { get; set; }

        public string ValidAudience { get; set; }
    }
}