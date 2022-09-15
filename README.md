# AspNetCoreIdentity

An ASP.NET Core 6.0 Identity service

# Database Setup

- Uses MySQL.
- Install/update dotnet ef tool:

```
$ dotnet tool install --global dotnet-ef
$ dotnet tool update --global dotnet-ef
```

- Apply database migrations to create the db. From a command line within the _Web.Api.Infrastructure_ project folder use the dotnet CLI to run :

```
$ cd Web.Api.Infrastructure
$ dotnet ef database update --context AppDbContext
$ dotnet ef database update --context AppIdentityDbContext
```

# Visual Studio

Open the solution file <code>AspNetCoreIdentity.sln</code> and build/run.

# Visual Studio Code

- `Ctrl`+`Shift`+`B` to build
- `F5` to start debug session

## Unit Testing

- Install .Net Core Test Explorer
- `echo fs.inotify.max_user_instances=524288 | sudo tee -a /etc/sysctl.conf && sudo sysctl -p`
  - https://github.com/dotnet/aspnetcore/issues/8449

# Swagger Enabled

To explore and test the available APIs simply run the project and use the Swagger UI @ http://localhost:{port}/swagger/index.html

The available APIs include:

# Continuous Integration:

- Integrated with CircleCI

# Kubernetes

- If ingress uses a prefix path, the prefix needs to be added as an environment variable `PATH_BASE` (or `appsettings.json` mounted from ConfigMap)
- Swagger does NOT work when the `PATH_BASE` is not `/` due to an issued filed as https://github.com/dotnet/aspnetcore/issues/42559
