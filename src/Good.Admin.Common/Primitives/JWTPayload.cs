using System;

namespace Good.Admin.Common.Primitives
{
    public class JWTPayload
    {
        public string UserId { get; set; }
        public DateTime Expire { get; set; }
    }
}
