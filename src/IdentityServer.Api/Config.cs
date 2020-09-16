using IdentityModel;
using IdentityServer.Core.Entities;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
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
            public static IEnumerable<IdentityResource> GetIdentityResources(IConfiguration configuration)
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
                    UserClaims = new[]{
                        ClaimTypes.Name,
                        ClaimTypes.Email,
                        ClaimTypes.GivenName,
                        ClaimTypes.Surname,
                        ClaimTypes.NameIdentifier,
                        ClaimTypes.Sid,
                        ClaimTypes.Role},
                },
                new IdentityResource
                {
                    Name = "ca",
                    DisplayName = "EA CA IAM",
                    Description = "Enterprise Architecture CA IAM"
                },
                new IdentityResource("roles", new[] { JwtClaimTypes.Role, ClaimTypes.Role })
                };
            }

            public static IEnumerable<ApiResource> GetApis(IConfiguration configuration)
            {
                return new ApiResource[]
                {
                    //api1
                    new ApiResource("invoices", "Invoices")
                    {
                        Enabled = true,
                        ApiSecrets =
                        {
                            new Secret
                            {
                                // Type must be "X509CertificateBase64"
                                Type = SecretTypes.X509CertificateBase64,

                                // base64 value of the api cert public key      
                                Value = Convert.ToBase64String(new X509Certificate2("Certificates/Api1.cer").GetRawCertData())
                            }
                        },
                        UserClaims = new[] {
                            configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"],
                            JwtClaimTypes.PreferredUserName
                        },
                        Scopes = new[]{"invoices.read" , "invoices.write", "invoices.delete", "invoices.update",  "otsuser" }
                    },

                    //api2
                    new ApiResource("inventory", "Inventory")
                    {
                        ApiSecrets =
                        {
                            new Secret
                            {
                                // Type must be "X509CertificateBase64"
                                Type = SecretTypes.X509CertificateBase64,

                                // base64 value of the api cert public key      
                                Value = Convert.ToBase64String(new X509Certificate2("Certificates/Api2.cer").GetRawCertData())
                            }
                        },
                        Enabled = true,
                        UserClaims = new[] {
                            configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"],
                             JwtClaimTypes.PreferredUserName
                        },
                        Scopes = new[] { "inventory.read", "inventory.write", "inventory.delete", "inventory.update" , "otsuser" }

                    },
                                       

                    //api3
                    new ApiResource("pendinginventory", "Pending Inventory")
                    {
                        Enabled = true,
                        ApiSecrets =
                        {
                            new Secret
                            {
                                // Type must be "X509CertificateBase64"
                                Type = SecretTypes.X509CertificateBase64,

                                // base64 value of the api cert public key      
                                Value = Convert.ToBase64String(new X509Certificate2("Certificates/Api3.cer").GetRawCertData())
                            }
                        },
                        UserClaims = new[] {
                            configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"],
                            JwtClaimTypes.PreferredUserName
                        },
                        Scopes = new[]{ "pendinginventory.read", "pendinginventory.write", "pendinginventory.delete", "pendinginventory.update",  "otsuser" }
                    },
                };
            }

            public static IEnumerable<ApiScope> GetApiScopes(IConfiguration configuration)
            {
                return new List<ApiScope>
                {
                    // invoice API 1 specific scopes
                    new ApiScope(name: "invoices.read",   displayName: "Read the invoices data.", userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),
                    new ApiScope(name: "invoices.write",  displayName: "Write to invoices data.",userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"]}),
                    new ApiScope(name: "invoices.delete", displayName: "Delete the invoices data.",userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),
                    new ApiScope(name: "invoices.update", displayName: "Update the invoices data.",userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"]}),

                    // invoice API 2 specific scopes
                    new ApiScope(name: "inventory.read",   displayName: "Read the inventory data.", userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),
                    new ApiScope(name: "inventory.write",  displayName: "Write to inventory data.",userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),
                    new ApiScope(name: "inventory.delete", displayName: "Delete the inventory data.",userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),
                    new ApiScope(name: "inventory.update", displayName: "Update the inventory data.",userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),

                    // invoice API 3 specific scopes
                    new ApiScope(name: "pendinginventory.read",   displayName: "Read the pending inventory data.", userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),
                    new ApiScope(name: "pendinginventor.write",  displayName: "Write to pending inventory data.",userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),
                    new ApiScope(name: "pendinginventor.delete", displayName: "Delete the pending inventory data.",userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),
                    new ApiScope(name: "pendinginventor.update", displayName: "Update the pending inventory data.",userClaims: new[] { configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),

                    new ApiScope(name: "otsuser", displayName: "The object Guid of the user",userClaims: new[] { JwtClaimTypes.PreferredUserName, configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"] }),

                  };
            }


            public static IEnumerable<Client> GetClients()
            {
                return new[]
                {
                    ////////////////////////////////////////////////
                    // Mvc Clients Authorization Code Flow and PKCE
                    ///////////////////////////////////////////////
                    //pkce
                    new Client
                        {
                            ClientId = "mvcClient.pkce",
                            ClientName = "MVC Pkce Client",
                            Description = "Client App using authorization code flow with pkce for client authentication",
                            EnableLocalLogin = false,
                            IdentityProviderRestrictions = new List<string>(){"adfs"},
                            AllowedGrantTypes = GrantTypes.CodeAndClientCredentials, //response type is "code"
                            //do not require a secret
                            RequireClientSecret = false,
                            IncludeJwtId = true,
                            AccessTokenType = AccessTokenType.Jwt,
                            RequireConsent = false,
                            RequirePkce = true,
                            AllowedCorsOrigins = { "https://localhost:5001" },
                            //AlwaysIncludeUserClaimsInIdToken= true,
                            AlwaysSendClientClaims= false,
                            UpdateAccessTokenClaimsOnRefresh = true,
                            Properties =  new Dictionary<string, string>
                            {
                                {"ClientType", ClientType.WebHybrid },
                                {"RawCertData", Convert.ToBase64String(new X509Certificate2("Certificates/MvcJwtClient.Web.cer").GetRawCertData())},
                                {"RequireJwe", true.ToString() }
                            },
                                            
                            // where to redirect to after login
                            RedirectUris = { "https://localhost:5001/signin-oidc" },

                            // where to redirect to after logout
                            PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                            AllowedScopes = new List<string>
                            {
                                StandardScopes.OpenId,
                                "invoices.read",
                                "invoices.write",
                                "inventory.read",
                                "roles",
                                "otsuser"
                            },

                            //Token
                            //Allow requesting refresh tokens for long lived API access                         
                            AllowOfflineAccess = true,
                            
                            //Access token life time example 7200 seconds (2 hour) - 2 x 60 x 60
                            //default is 3600 => 1 hour
                            AccessTokenLifetime = 60,
                            //Identity token life time example 7200 seconds (2 hour)
                            //default is 300 =>  5 minutes
                            IdentityTokenLifetime = 60,

                            RefreshTokenUsage = TokenUsage.ReUse
                        },

                    //mtls
                    new Client
                        {
                            ClientId = "mvcClient.mtls",
                            ClientName = "MVC Mtls Client",
                            Description = "Client App using Mtls or Client Authentication",
                            ClientSecrets = {
                                // name based- do this is production so it won't be chaos when cert expires
                                //new Secret(@"CN=mtls.test, OU=ROO\ballen@roo, O=mkcert development certificate", "mtls.test")
                                //{
                                //    Type = SecretTypes.X509CertificateName
                                //},
                                //thumbprint based
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
                            IncludeJwtId = true,
                            RequirePkce = false,    //https://www.scottbrady91.com/OpenID-Connect/ASPNET-Core-using-Proof-Key-for-Code-Exchange-PKCE
                            AllowedCorsOrigins = { "https://localhost:5001" },
                            AlwaysIncludeUserClaimsInIdToken= false,
                            AlwaysSendClientClaims= false,
                            UpdateAccessTokenClaimsOnRefresh = true,                           
                            // where to redirect to after login
                            RedirectUris = { "https://localhost:5001/signin-oidc" },
                          
                             // where to redirect to after logout
                            PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                            AllowedScopes = new List<string>
                            {
                                 StandardScopes.OpenId,
                                "invoices.read",
                                "invoices.write",
                                "inventory.read",
                                "roles",
                                "otsuser"
                            },                          
                  
                            //Token
                            //Allow requesting refresh tokens for long lived API access                         
                            AllowOfflineAccess = true,
                            
                            //Access token life time example 7200 seconds (2 hour) - 2 x 60 x 60
                            //default is 3600 => 1 hour
                            AccessTokenLifetime = 60,
                            //Identity token life time example 7200 seconds (2 hour)
                            //default is 300 =>  5 minutes
                            IdentityTokenLifetime = 60,

                            RefreshTokenUsage = TokenUsage.ReUse
                        },

                    /////////////////////////////////////////////////////////////////
                    // Mvc Clients Authrorization Code Flow with client JWT assertion
                    /////////////////////////////////////////////////////////////////
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

                        AllowedGrantTypes = GrantTypes.Hybrid,
                        RedirectUris = {"https://localhost:5001/signin-oidc"},
                     
                        // where to redirect to after logout
                        PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                        AllowedScopes = {"openid", "profile", "api1", "api2" },
                        AllowOfflineAccess = true,
                        AllowAccessTokensViaBrowser = true //this is dangerous since access_token will appear in browser
                    },                   

                    ///////////////////////////////////////////////////////////
                    // Console Client Credentials Flow with client JWT assertion
                    ///////////////////////////////////////////////////////////
                    new Client
                    {
                        ClientId = "client.jwt",
                        ClientName = "Console Jwt Client",
                        Description = "Console app using JWT Bearer Token for Client Authentication",
                        ClientSecrets =
                        {
                            new Secret
                            {
                                Type = SecretTypes.X509CertificateBase64,
                                Value = Convert.ToBase64String(new X509Certificate2("Certificates/MvcJwtClient.Web.cer").GetRawCertData())
                            }
                        },

                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        AllowedScopes = { "invoices.read"}
                    },

                     new Client
                        {
                            ClientId = "api2",
                            ClientName = "Api2 Pkce as Client",
                            Description = "Api using authorization code flow with pkce for api authentication",
                            AllowedGrantTypes = { "delegation" , GrantType.ClientCredentials},
                            AllowedScopes = new List<string>
                             {
                                 "api3"
                             },
                            ClientSecrets =
                            {
                                new Secret
                                {
                                    // Type must be "X509CertificateBase64"
                                    Type = SecretTypes.X509CertificateBase64,

                                    // base64 value of the client cert public key      
                                    Value = Convert.ToBase64String(new X509Certificate2("Certificates/Api2.cer").GetRawCertData())
                                }
                            },
                            RequirePkce = false
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