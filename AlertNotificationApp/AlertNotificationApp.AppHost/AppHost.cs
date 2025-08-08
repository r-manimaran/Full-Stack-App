using Google.Protobuf.WellKnownTypes;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlite("db")    
                .WithSqliteWeb();

// configure Keycloak authentication
var username = builder.AddParameter("keycloak-username", "admin")
                      .WithDescription("Keycloak admin username");

var password = builder.AddParameter("keycloak-password", "admin", secret: true);


var keycloak = builder.AddKeycloak("keycloak", 8080, username, password)
                      .WithDataVolume()
                      .WithExternalHttpEndpoints();
                      //.WithRealmImport("./KeycloakConfiguration");

var apiService = builder.AddProject<Projects.WebApi>("webapi")
                        .WithExternalHttpEndpoints()
                        .WithReference(db)
                        .WithReference(keycloak)
                        .WaitFor(db)
                        .WaitFor(keycloak)
                        .WithEnvironment("Keycloak__ClientId", "confidential-client")
                        .WithEnvironment("Keycloak__ClientSecret", "ze4SQDpbyBlB72kdTCTv8ecSWsJHf2Js");

builder
    .AddNpmApp("AngularFrontEnd", "../AlertNotificationWebApp")
    .WithReference(apiService)
    .WithEndpoint(4200, scheme: "http", env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
