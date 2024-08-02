// See https://aka.ms/new-console-template for more information
using IdentityModel.Client;
using System.Text.Json;

Console.WriteLine("Hello, World!");
var client = new HttpClient();
var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
if (discovery.IsError)
    Console.WriteLine(discovery.Error);
// Request a token from IdentityServer
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = discovery.TokenEndpoint,
    ClientId = "client1",
    ClientSecret = "client1secret",
    Scope = "api1"
});
Console.WriteLine(tokenResponse.IsError ? tokenResponse.Error : $"Successfully retrieved token from IdentityServer: {tokenResponse.AccessToken}");
// Call the API
var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);
var response = await apiClient.GetAsync("https://localhost:5001/webapi");
if (!response.IsSuccessStatusCode)
    Console.WriteLine($"Failed to access webapi endpoint! {response.StatusCode}");
else
{
    var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}
Console.ReadLine();