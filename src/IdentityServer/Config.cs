using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId()
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
                ClientSecrets = { new Secret("client1".Sha512())},
                // scopes that this client has access to.
                AllowedScopes = {"api1"}
            }
        };
}