using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// AddAuthentication registers the authentication services. Notice that in its options, the DefaultChallengeScheme is set to “oidc”, and the DefaultScheme is set to “Cookies”.
// The DefaultChallengeScheme is used when an unauthenticated user must log in. This begins the OpenID Connect protocol, redirecting the user to IdentityServer.
// After the user has logged in and been redirected back to the client, the client creates its own local cookie.
// Subsequent requests to the client will include this cookie and be authenticated with the default Cookie scheme.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies") // After the call to AddAuthentication, AddCookie adds the handler that can process the local cookie.
    .AddOpenIdConnect("oidc", options => //  AddOpenIdConnect is used to configure the handler that performs the OpenID Connect protocol.
    {
        options.Authority = "https://localhost:5000"; // where the trusted token service is located.
        options.ClientId = "openidclient";
        options.ClientSecret = "openidsecret";
        options.ResponseType = "code";
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email_verification");
        options.Scope.Add("phone_verification");
        options.Scope.Add("api1");
        options.Scope.Add("offline_access");
        options.ClaimActions.MapJsonKey("email_verified", "email_verified"); // map the new claim returned from the userinfo endpoint onto a user claim.
        options.ClaimActions.MapJsonKey("phone_verified", "phone_verified"); // map the new claim returned from the userinfo endpoint onto a user claim.
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true; // automatically store the id, access, and refresh tokens in the properties of the authentication cookie.
    });
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
