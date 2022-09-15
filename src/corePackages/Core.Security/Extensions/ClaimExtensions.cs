using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Core.Security.Extensions
{
    public static class ClaimExtensions
    {

        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddName(this ICollection<Claim> claims, string firstName,string lastName)
        {
            claims.Add(new Claim(ClaimTypes.Name, firstName + " " + lastName));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            foreach (string role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
        }

        public static void AddRoles(this ICollection<Claim> claims, IEnumerable<string> roles)
        {
            foreach (string role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
        }

    }

}