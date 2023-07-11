using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.ExtensionMethods
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetCurrentUserName(this ClaimsPrincipal user) //NameId == ClaimTypes.Name
        {
            return user.FindFirst("name")?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value; // ClaimTypes.NameIdentifier == UniqueName
            
        }
    }
}