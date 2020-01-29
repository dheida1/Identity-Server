using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.Api
{
    namespace IdentityServer.Api
    {
        //https://stackoverflow.com/questions/45574821/identityserver4-persistedgrantdbcontext-configurationdbcontext
        public static class Config
        {
            public static IEnumerable<IdentityResource> GetIdentityResources()
            {
                return new IdentityResource[]
                {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "google",
                    DisplayName = "Google Organization",
                    Description = "Google user accounts",
                    UserClaims = new List<string> {"hd"},
                },
                new IdentityResource
                {
                    Name = "adfs",
                    DisplayName = "LA State ADFS",
                    Description = "LA State ADFS",
                },
                new IdentityResource
                {
                    Name = "ca",
                    DisplayName = "EA CA IAM",
                    Description = "Enterprise Architecture CA IAM",
                },
                new IdentityResource
                {
                    Name = "rogue_adfs",
                    DisplayName = "LA OTS ADFS",
                    Description = "LA OTS ADFS",
                },

                  new IdentityResource("roles", new[] { JwtClaimTypes.Role  })
                };
            }

            public static IEnumerable<ApiResource> GetApis()
            {
                return new ApiResource[]
                {
                    new ApiResource("api1", "My API #1"),

                    new ApiResource("test_api", "My Test API")
                    {
                      ApiSecrets = { new Secret("secret".Sha256()) },
                      UserClaims =  { ClaimTypes.Name, ClaimTypes.Email}
                    },
                    new ApiResource("my_account_user", "My Account User API")
                    {
                        ApiSecrets = { new Secret("secret".Sha256()) },
                    }
                };
            }

            public static IEnumerable<Client> GetClients()
            {
                return new[]
                {
                    new Client
                        {
                            ClientId = "mvc",
                            ClientName = "MVC Web Client",
                            ClientSecrets = { new Secret("secret".Sha256()) },
                            EnableLocalLogin = false,
                            IdentityProviderRestrictions = new List<string>(){"adfs"},

                            AllowedGrantTypes = GrantTypes.Code,
                            AccessTokenType = AccessTokenType.Jwt,
                            RequireConsent = false,
                            RequirePkce = false,
                            AllowedCorsOrigins = { "https://localhost:5001" },
                            AlwaysIncludeUserClaimsInIdToken= true,
                            AlwaysSendClientClaims= true,
                            ClientClaimsPrefix = "",
                
                            // where to redirect to after login
                            RedirectUris = { "https://localhost:5001/signin-oidc" },

                            // where to redirect to after logout
                            PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                            AllowedScopes = new List<string>
                            {
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile
                            },
                            AllowAccessTokensViaBrowser = true,
                            AllowOfflineAccess = true
                        },
                    // client credentials flow client
                    new Client
                        {
                            ClientId = "client",
                            ClientName = "Client Credentials Client",

                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                            AllowedScopes = { "api1" }
                        },

                         // MVC client using hybrid flow
                    new Client
                        {
                            ClientId = "mvcCert",
                            ClientName = "MVC Client",
                            AllowedGrantTypes = GrantTypes.Code,
                   
                    
                            //ClientSecrets =  { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                            AccessTokenType = AccessTokenType.Jwt,
                            ClientSecrets = {
                            //new Secret(@"CN=mtls.test, OU=ROO\ballen@roo, O=mkcert development certificate", "mtls.test")
                            //{
                            //    Type = SecretTypes.X509CertificateName
                            //},
                                 // or:
                                 new Secret("01E6592143E1413907469C3669D6F4EC6E582C05", "MvcClient.cert")
                                 {// serial number is 1E0000428DD2559EBA25D96B8600000000428D
                                     Type = SecretTypes.X509CertificateThumbprint
                                 },
                                 //new Secret
                                 //   {
                                 //       // Type must be "X509CertificateBase64"
                                 //       Type = IdentityServerConstants.SecretTypes.X509CertificateBase64,

                                 //       // base64 value of the public key      
                                 //      Value = Convert.ToBase64String(new X509Certificate2(new Cryptography.X509Certificates.Extension.X509Certificate2(
                                 //          "1E0000428DD2559EBA25D96B8600000000428D",
                                 //          StoreName.My, StoreLocation.LocalMachine, X509FindType.FindBySerialNumber
                                 //      )).GetRawCertData())
                                 //   }
                             },
                            EnableLocalLogin = false,
                            IdentityProviderRestrictions = new List<string>(){"rogue_adfs"},
                            RequireConsent = false,

                            RedirectUris = { "https://localhost:5002/signin-oidc" },
                            //FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",
                            PostLogoutRedirectUris = { "https://localhost:5002/" },
                            //PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                            AllowedCorsOrigins = { "https://localhost:5002" },
                            //AlwaysSendClientClaims= true,
                            AlwaysIncludeUserClaimsInIdToken= true,
                            AlwaysSendClientClaims= true,
                            ClientClaimsPrefix = "",
                            RequirePkce = false,//https://www.scottbrady91.com/OpenID-Connect/ASPNET-Core-using-Proof-Key-for-Code-Exchange-PKCE
                            AllowPlainTextPkce = false,
                            AllowedScopes = new List<string>
                            {
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile,
                                ClaimTypes.Role,
                                "test_api",
                                "roles"
                            },
                  
                            //Allow requesting refresh tokens for long lived API access
                            AllowAccessTokensViaBrowser = true,
                            AllowOfflineAccess = true
                        },   

                    // SPA client using implicit flow
                    new Client
                        {
                            ClientId = "spa",
                            ClientName = "SPA Client",
                            ClientUri = "http://identityserver.io",

                            AllowedGrantTypes = GrantTypes.Implicit,
                            AllowAccessTokensViaBrowser = true,

                            RedirectUris =
                            {
                                "http://localhost:5002/index.html",
                                "http://localhost:5002/callback.html",
                                "http://localhost:5002/silent.html",
                                "http://localhost:5002/popup.html",
                            },

                            PostLogoutRedirectUris = { "http://localhost:5002/index.html" },
                            AllowedCorsOrigins = { "http://localhost:5002" },

                            AllowedScopes = { "openid", "profile", "api1" }
                        }
                };
            }
        }
    }
}