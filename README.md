# AspNetCoreIdentity

An ASP.NET Core 8.0 Identity service

# Database Setup

- Uses MySQL.
- Install/update dotnet ef tool:

```
$ dotnet tool install --global dotnet-ef
$ dotnet tool update --global dotnet-ef
```

- Either run the application / `AspNetCoreIdentity.Server` project and the DB will be automatically created or:
- Apply database migrations to create the db. From a command line within the `AspNetCoreIdentity.Server` project folder use the dotnet CLI to run :

```
$ cd AspNetCoreIdentity.Server
$ dotnet ef database update --context PersistedGrantDbContext
$ dotnet ef database update --context ConfigurationDbContext
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
