using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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
                new IdentityResources.Email(),
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
                    UserClaims = new[]{ ClaimTypes.Name,
                        ClaimTypes.Email,
                        ClaimTypes.GivenName,
                        ClaimTypes.Surname,
                        ClaimTypes.NameIdentifier,
                        ClaimTypes.Sid,
                        ClaimTypes.Role}
                },
                new IdentityResource
                {
                    Name = "ca",
                    DisplayName = "EA CA IAM",
                    Description = "Enterprise Architecture CA IAM"
                },
                new IdentityResource("roles", new[] { JwtClaimTypes.Role  })

                };
            }

            public static IEnumerable<ApiResource> GetApis()
            {
                return new ApiResource[]
                {
                    new ApiResource("api1", "My API #1"),

                    new ApiResource("api2",  "My API #2")
                    {
                      ApiSecrets = { new Secret("secret".Sha256()) },
                      UserClaims = new [] { ClaimTypes.Name,
                          ClaimTypes.Email,
                          ClaimTypes.Name,
                          ClaimTypes.Role,
                          JwtClaimTypes.Name,
                          JwtClaimTypes.Email,
                          JwtClaimTypes.Role
                      },
                      Scopes = new[]
                          {
                           new Scope
                              {
                                    Name = "api2",
                                    DisplayName = "Full access to Api2",
                                    UserClaims = new[] {"emails" }
                              },
                              new Scope
                              {
                                    Name = "api2.full_access",
                                    DisplayName = "Full access to Api2",
                                    UserClaims = new[] { JwtClaimTypes.Role,
                                        ClaimTypes.Role,
                                        ClaimTypes.Name,
                                        ClaimTypes.Email,
                                        JwtClaimTypes.Name,
                                        JwtClaimTypes.Email
                                    },
                              },
                               new Scope
                              {
                                    Name = "api2.read_only",
                                    DisplayName = "Read only access to Api2",
                                    UserClaims = new[] { JwtClaimTypes.Role,
                                        ClaimTypes.Role,  ClaimTypes.Name,
                                        ClaimTypes.Email, JwtClaimTypes.Name,
                                        JwtClaimTypes.Email
                                    },
                              }
                          }
                    }
                };
            }

            public static IEnumerable<Client> GetClients()
            {
                return new[]
                {
                    //pkce
                    new Client
                        {
                            ClientId = "mvcClient.pkce",
                            ClientName = "MVC Pkce Client",
                            Description = "Client App using authorization code flow with pkce for client authentication",
                            EnableLocalLogin = false,
                            IdentityProviderRestrictions = new List<string>(){"adfs"},
                            AllowedGrantTypes = GrantTypes.CodeAndClientCredentials, //response type is "code"
                            RequireClientSecret = false,
                            AccessTokenType = AccessTokenType.Jwt,
                            RequireConsent = false,
                            RequirePkce = true,
                            AllowedCorsOrigins = { "https://localhost:5001" },
                            AlwaysIncludeUserClaimsInIdToken= true,
                            AlwaysSendClientClaims= true,
                            UpdateAccessTokenClaimsOnRefresh = true,                         
                                            
                            // where to redirect to after login
                            RedirectUris = { "https://localhost:5001/signin-oidc" },

                            // where to redirect to after logout
                            PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                            AllowedScopes = new List<string>
                            {
                                StandardScopes.OpenId,
                                StandardScopes.Profile,
                                StandardScopes.Email,
                                "api1",
                                "api2",
                                "roles"
                            },
                            //Allow requesting refresh tokens for long lived API access                         
                            AllowOfflineAccess = true
                        },

                    //mtls
                    new Client
                        {
                            ClientId = "mvcClient.mtls",
                            ClientName = "MVC Mtls Client",
                            Description = "Client App using Mtls or Client Authentication",
                            ClientSecrets = {
                                 new Secret("2767798A6DC7691C8EF41414BF7C9D59DB9DA31A", "MvcClient.Web")
                                 {
                                     Type = SecretTypes.X509CertificateThumbprint
                                 },
                             },
                            EnableLocalLogin = false,
                            IdentityProviderRestrictions = new List<string>(){"adfs"},

                            AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                            AccessTokenType = AccessTokenType.Jwt,
                            RequireConsent = false,
                            RequirePkce = false,    //https://www.scottbrady91.com/OpenID-Connect/ASPNET-Core-using-Proof-Key-for-Code-Exchange-PKCE
                            AllowedCorsOrigins = { "https://localhost:5001" },
                            AlwaysIncludeUserClaimsInIdToken= true,
                            UpdateAccessTokenClaimsOnRefresh = true,                           
                            // where to redirect to after login
                            RedirectUris = { "https://localhost:5001/signin-oidc" },
                          
                             // where to redirect to after logout
                            PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                            AllowedScopes = new List<string>
                            {
                                StandardScopes.OpenId,
                                StandardScopes.Profile,
                                StandardScopes.Email,
                                "api1",
                                "api2"
                            },                          
                  
                            //Allow requesting refresh tokens for long lived API access
                            AllowAccessTokensViaBrowser = true,
                            AllowOfflineAccess = true
                        },

                    //jwt
                    new Client
                    {
                        ClientId = "mvcClient.jwt",
                        ClientName = "Mvc Jwt Client",
                        Description = "Client App using JWT Bearer Token for Client Authentication",
                        EnableLocalLogin = false,
                        IdentityProviderRestrictions = new List<string>(){"adfs"},
                        RequireConsent = false,
                        AlwaysIncludeUserClaimsInIdToken= true,
                        ClientSecrets =
                        {
                            new Secret
                            {
                                // Type must be "X509CertificateBase64"
                                Type = SecretTypes.X509CertificateBase64,

                                // base64 value of the client cert public key      
                                Value = Convert.ToBase64String(new X509Certificate2("Certificates/MvcJwtClient.Web.cer").GetRawCertData())
                            }
                        },

                        AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                        RedirectUris = {"https://localhost:5001/signin-oidc"},
                     
                        // where to redirect to after logout
                        PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                        AllowedScopes = {"openid", "profile", "api1", "api2" },
                        AllowOfflineAccess = true
                    },

                    ///////////////////////////////////////////
                    // Console Client Credentials Flow with client JWT assertion
                    //////////////////////////////////////////
                    new Client
                    {
                        ClientId = "client.jwt",
                        ClientName = "Console Jwt Client",
                        Description = "Console app using JWT Bearer Token for Client Authentication",
                        ClientSecrets =
                        {
                            new Secret
                            {
                                Type = IdentityServerConstants.SecretTypes.X509CertificateBase64,
                                Value = Convert.ToBase64String(new X509Certificate2("Certificates/MvcJwtClient.Web.cer").GetRawCertData())
                            }
                        },

                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        AllowedScopes = { "api1", "api2" }
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