using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AspNetCoreIdentity.Server;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        bool _isIntegrationTests = !string.IsNullOrEmpty(environment) && environment.Equals("IntegrationTests");

        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();
        if (_isIntegrationTests)
            builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(TestUsers.Users);
        else
            builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            }).AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = dbContext => dbContext.UseMySql(builder.Configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default")), b => b.MigrationsAssembly("AspNetCoreIdentity.Server"));
            }).AddOperationalStore(options =>
            {
                options.ConfigureDbContext = dbContext => dbContext.UseMySql(builder.Configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default")), b => b.MigrationsAssembly("AspNetCoreIdentity.Server"));
            }).AddTestUsers(TestUsers.Users);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();
        InitializeDatabase(app);
        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
    private static void InitializeDatabase(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            try
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var configService = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                configService.Database.Migrate();
                if (!configService.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                        configService.Clients.Add(client.ToEntity());
                    configService.SaveChanges();
                }
                if (!configService.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                        configService.IdentityResources.Add(resource.ToEntity());
                    configService.SaveChanges();
                }
                if (!configService.ApiScopes.Any())
                {
                    foreach (var scope in Config.ApiScopes)
                        configService.ApiScopes.Add(scope.ToEntity());
                    configService.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{nameof(InitializeDatabase)} Exception! ${e}");
            }
    }
}
