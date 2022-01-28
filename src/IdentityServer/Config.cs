using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api1","My API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "client",
                    // 没有交互式用户，使用 clientid/secret 进行身份验证
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //用于身份验证的密钥
                    ClientSecrets = {new Secret("secret".Sha256())},
                    // 客户端有权访问的范围
                    AllowedScopes = {"api1"}
                },
                // 交互式 ASP.NET Core MVC 客户端
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                  

                    //登陆后重定向到哪里
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    //注销后重定向到哪里
                    PostLogoutRedirectUris = {"https://localhost:5002/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = new List<string>()
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }

                }

            };
    }
}