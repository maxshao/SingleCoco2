using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCoco.Dtos.Query.Jwt
{
    public class JwtAuthorizationDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public long Auths { get; set; }
        public long Expires { get; set; }
        public bool Success { get; set; }
    }
}