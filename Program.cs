using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using GraphQLOracleApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OracleDbContext>(
    options => options.UseOracle( builder.Configuration.GetConnectionString("OracleConnection")
        ?.Replace("%ORACLE_PASSWORD%", builder.Configuration["ORACLE_PASSWORD"])),
    ServiceLifetime.Singleton,
    ServiceLifetime.Singleton
);

// Add other services (GraphQL, etc.)
builder.Services
    .AddGraphQLServer()
        .AddQueryType<QueryType>()
        .AddMutationType<MutationType>();

var app = builder.Build();

// Seed the database with test data
var dbContext = app.Services.GetRequiredService<OracleDbContext>();
var seedData = new SeedData(dbContext);
seedData.SeedDatabase();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseRouting();

app.MapGraphQL("/graphql");

app.MapGet("/ip", async context =>
{
    using var client = new HttpClient();
    var response = await client.GetAsync("https://api.ipify.org?format=json");
    response.EnsureSuccessStatusCode();
    
    var body = await response.Content.ReadAsStringAsync();
    
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync(body);
});

app.MapGet("/cfg", async context =>
{
    context.Response.ContentType = "application/json";
    var configurationRoot = (IConfigurationRoot)builder.Configuration;
    await context.Response.WriteAsJsonAsync(new
    {
        Sources = configurationRoot.Providers.Select((provider, index) =>
        {
            var data = new Dictionary<string, string?>();
            provider.TryGet("", out var _); // Trigger the provider to load data if not already loaded
            foreach (var key in configurationRoot.AsEnumerable().Select(kvp => kvp.Key).Distinct())
            {
                if (provider.TryGet(key, out var value))
                {
                    data[key] = value;
                }
            }

            return new
            {
                Precedence = index + 1,
                Name = provider.ToString(),
                Data = data
            };
        }).ToList()
    });
});

app.MapGet("/env", async context =>
{
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsJsonAsync(new
    {
        Environment = context.RequestServices.GetRequiredService<IHostEnvironment>().EnvironmentName,
        EnvironmentVariables = context.RequestServices.GetRequiredService<IConfiguration>().AsEnumerable().Select(v => new { v.Key, v.Value}).ToList()
    });
});

app.MapGet("/svc", async context =>
{
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsJsonAsync(new
    {
        Environment = context.RequestServices.GetRequiredService<IHostEnvironment>().EnvironmentName,
        Services = builder.Services.Select(s => s.ServiceType.FullName).ToList()
    });
});

app.Run();
