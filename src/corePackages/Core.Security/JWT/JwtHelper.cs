using Core.Security.Encryption;
using Core.Security.Entities;
using Core.Security.Extensions;
using Core.Security.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; set; }
        private TokenOptions TokenOptions { get; }
        private DateTime AccessTokenExpiration { get; set; }


        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            TokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }


        public RefreshToken CreateRefreshToken(User user, string ipAddress)
        {
            return new RefreshToken()
            {
                UserId = user.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public AccessToken CreateToken(User user, IList<OperationClaim> operationClaims)
        {
            AccessTokenExpiration = DateTime.Now.AddMinutes(TokenOptions.AccessTokenExpiration);
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(TokenOptions.SecurityKey);
            SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwtSecurityToken = CreateJwtSecurityToken(TokenOptions, user, signingCredentials, operationClaims);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            string? token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return new AccessToken
            {
                Token = token,
                Expiration = AccessTokenExpiration
            };
        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, IList<OperationClaim> operationClaims)
        {
            return new JwtSecurityToken
                (
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: AccessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
                );
        }

        private IEnumerable<Claim> SetClaims(User user, IList<OperationClaim> operationClaims)
        {
            List<Claim> claims = new();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName(user.FirstName, user.LastName);
            claims.AddRoles(operationClaims.Select(o => o.Name));
            return claims;
        }
    }
}
