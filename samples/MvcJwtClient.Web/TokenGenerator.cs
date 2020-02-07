using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace MvcJwtClient.Web
{
    public static class TokenGenerator
    {

        public static string CreateClientAuthJwt()
        {
            // set exp to 5 minutes
            var tokenHandler = new JwtSecurityTokenHandler { TokenLifetimeInMinutes = 5 };

            var securityToken = tokenHandler.CreateJwtSecurityToken(
                // iss must be the client_id of our application
                issuer: "MvcJwtClient",
                // aud must be the identity provider (token endpoint)
                audience: "https://localhost:4300/connect/token",
                // sub must be the client_id of our application
                subject: new ClaimsIdentity(
                  new List<Claim> { new Claim("sub", "MvcJwtClient") }),
                // sign with the private key (using RS256 for IdentityServer)
                signingCredentials: new SigningCredentials(
                  new X509SecurityKey(new X509Certificate2("Certificates/MvcClient.Web.pfx", "1234")), "RS256")
            );
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
