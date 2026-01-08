using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Data;
using WebApi.Endpoints;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqliteDbContext<AppDbContext>("db");

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddKeycloakJwtBearer(JwtBearerDefaults.AuthenticationScheme, realm: "maransys", jwtOptions =>
    {
        jwtOptions.Audience = "dotnet-api";
        jwtOptions.RequireHttpsMetadata = false; // Allow HTTP for development (Keycloak on port 8081)
        jwtOptions.SaveToken = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1"));

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/home",()=> "Welcome to the Web API!").WithName("Home").WithTags("Home");

await app.ApplyMigrations();

app.MapProductEndpoints();

app.MapFeatureNotificationEndpoints();

await app.RunAsync();

