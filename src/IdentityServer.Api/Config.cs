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
                                IdentityServerConstants.StandardScopes.Profile,
                                "api1"
                            },
                            //Allow requesting refresh tokens for long lived API access
                            AllowAccessTokensViaBrowser = true,
                            AllowOfflineAccess = true
                        },  
                         // MVC client using hybrid flow
                    new Client
                        {
                            ClientId = "mvcCert",
                            ClientName = "MVC Client",
                            ClientSecrets = {
                                 new Secret("2767798A6DC7691C8EF41414BF7C9D59DB9DA31A", "MvcClient.Web")
                                 {
                                     Type = SecretTypes.X509CertificateThumbprint
                                 },
                             },
                            EnableLocalLogin = false,
                            IdentityProviderRestrictions = new List<string>(){"adfs"},

                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            //GrantTypes.Code,
                            AccessTokenType = AccessTokenType.Jwt,
                            RequireConsent = false,
                            RequirePkce = false,    //https://www.scottbrady91.com/OpenID-Connect/ASPNET-Core-using-Proof-Key-for-Code-Exchange-PKCE
                            AllowedCorsOrigins = { "https://localhost:5001" },
                            AlwaysIncludeUserClaimsInIdToken= true,
                            AlwaysSendClientClaims= true,
                            ClientClaimsPrefix = "",
                            //UpdateAccessTokenClaimsOnRefresh = true,
                             // where to redirect to after login
                            RedirectUris = { "https://localhost:5001/signin-oidc" },
                            
                             // where to redirect to after logout
                            PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                            AllowedScopes = new List<string>
                            {
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Profile,
                                "api1"
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