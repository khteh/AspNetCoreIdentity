using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace AspNetCoreIdentity.Server;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "email_verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified,
                }
            },
            new IdentityResource()
            {
                Name = "phone_verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.PhoneNumberVerified,
                }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope(name: "api1", displayName: "API1")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client {
                ClientId = "client1",
                // no interactive user, use the clientId/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // secret for authentication
                ClientSecrets = { new Secret("client1secret".Sha512())},
                // scopes that this client has access to.
                AllowedScopes = {"api1"}
            },
            new Client {
                ClientId = "openidclient",
                ClientSecrets = { new Secret("openidsecret".Sha512())},
                AllowedGrantTypes = GrantTypes.Code,
                // Where to redirect to after login
                RedirectUris = { "https://localhost:5003/signin-oidc"},
                // Where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "email_verification", "phone_verification"
                }
            }
        };
}