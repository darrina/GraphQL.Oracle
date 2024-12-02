using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQLOracleApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OracleDbContext>(
    options => options.UseOracle( builder.Configuration.GetConnectionString("OracleConnection")),
    ServiceLifetime.Singleton,
    ServiceLifetime.Singleton
);

// Add other services (GraphQL, etc.)
builder.Services
    .AddGraphQLServer()
        .AddQueryType<QueryType>()
        .AddMutationType<MutationType>();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseRouting();

app.MapGraphQL("/graphql");

app.Run();
