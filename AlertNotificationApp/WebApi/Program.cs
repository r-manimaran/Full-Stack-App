using WebApi.Data;
using WebApi.Endpoints;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqliteDbContext<AppDbContext>("db");

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1"));

await app.ApplyMigrations();

app.MapProductEndpoints();

app.MapFeatureNotificationEndpoints();

app.UseHttpsRedirection();

await app.RunAsync();

