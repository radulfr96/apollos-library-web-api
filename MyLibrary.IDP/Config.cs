// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MyLibrary.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources
        {
            get
            {
                return new List<IdentityResource>()
                {
                    new IdentityResources.OpenId(),
                };
            }
        }

        public static IEnumerable<ApiResource> ApiResources =>
                new ApiResource[]
                {
                new ApiResource(
                    "mylibraryapi",
                    "My Library API",
                    new List<string>() { "role" })
                {
                    ApiSecrets = { new Secret("apisecret".Sha256()) }
                },
                };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    AccessTokenLifetime = 120,
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName = "My Library",
                    ClientId = "mylibrarywebclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:5050/signin-oidc"
                    },
                    RequirePkce = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "mylibraryapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256()),
                    }
                }
            };
    }
}