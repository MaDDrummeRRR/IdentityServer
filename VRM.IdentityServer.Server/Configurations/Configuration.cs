﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace VRM.IdentityServer.Server.Configurations
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.grandma"
                    }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope(name: "read",   displayName: "Read data."),
                new ApiScope(name: "write",  displayName: "Write data."),
                new ApiScope(name: "delete", displayName: "Delete data.")
            };
        }

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("ApiOne", "This is ApiOne resource")
                {
                    Scopes = { "read", "write", "delete" }
                },
                new ApiResource("ApiTwo", new string[] { "rc.api.grandma" })
                {
                    Scopes = { "read", "write" }
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
                new Client {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    AllowedScopes = new List<string>
                    {
                        "read",
                        "write",
                        "delete"
                    }
                },
                new Client {
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    RedirectUris = { "https://localhost:8001/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:8001/Home/Index" },

                    AllowedScopes = {
                        "ApiOne",
                        "ApiTwo",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "rc.scope",
                    },

                    // Положить все клаймы пользователя в Id Token
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                },
                new Client {
                    ClientId = "client_id_js",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "https://localhost:9001/home/signin" },
                    PostLogoutRedirectUris = { "https://localhost:9001/Home/Index" },
                    AllowedCorsOrigins = { "https://localhost:9001" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                        "ApiTwo",
                        "rc.scope",
                    },

                    AccessTokenLifetime = 1,

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },

                new Client {
                    ClientId = "angular",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "http://localhost:4200" },
                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    AllowedCorsOrigins = { "http://localhost:4200" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },
                new Client {
                    ClientId = "wpf",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "http://localhost/sample-wpf-app" },
                    AllowedCorsOrigins = { "http://localhost" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },
                new Client {
                    ClientId = "xamarin",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "xamarinformsclients://callback" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },
                new Client {
                    ClientId = "flutter",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "http://localhost:4000/" },
                    AllowedCorsOrigins = { "http://localhost:4000" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                }
            };
    }
}