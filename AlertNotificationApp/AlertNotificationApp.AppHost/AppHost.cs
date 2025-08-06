var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlite("db")    
                .WithSqliteWeb();


var apiService = builder.AddProject<Projects.WebApi>("webapi")
                        .WithReference(db);
                        

builder.Build().Run();
