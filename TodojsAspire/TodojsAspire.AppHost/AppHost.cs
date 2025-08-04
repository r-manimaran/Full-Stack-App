var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var db = builder.AddSqlite("db")
                .WithSqliteWeb();

var apiService = builder.AddProject<Projects.TodojsAspire_ApiService>("apiservice")
                        .WithReference(db)
                        .WithHttpHealthCheck("/health");

builder.AddViteApp(name: "todo-frontend", workingDirectory: "../todo-frontend")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithNpmPackageInstallation();

builder.Build().Run();
