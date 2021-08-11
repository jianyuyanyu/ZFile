using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Extensions.Authorize
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public const string JwtAuthenticationScheme = "JwtAuthenticationScheme";

        public JwtAuthorizeAttribute()
        {
            this.AuthenticationSchemes = JwtAuthenticationScheme;
        }
    }
}
