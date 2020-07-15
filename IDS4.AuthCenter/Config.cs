// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace IDS4.AuthCenter
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("orderApi","订单服务")
                {
                    ApiSecrets ={ new Secret("orderApi secret".Sha256()) },
                    Scopes = { "orderApiScope" }
                },
                new ApiResource("productApi","产品服务")
                {
                    ApiSecrets ={ new Secret("productApi secret".Sha256()) },
                    Scopes = { "productApiScope" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("orderApiScope"),
                new ApiScope("productApiScope"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "web client",
                    ClientName = "Web Client",

                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("web client secret".Sha256()) },

                    RedirectUris = { "http://localhost:5000/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5000/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5000/signout-callback-oidc" },

                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "orderApiScope", "productApiScope"
                    },
                    AllowAccessTokensViaBrowser = true,

                    RequireConsent = true,//是否显示同意界面
                    AllowRememberConsent = false,//是否记住同意选项
                },
                new Client
                {
                    ClientId = "postman client",
                    ClientName = "Postman Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("postman client secret".Sha256()) },

                    AllowedScopes = new [] {"orderApiScope", "productApiScope"},
                },
            };
    }
}