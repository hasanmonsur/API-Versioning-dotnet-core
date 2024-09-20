using BooksAPI.Contacts;
using BooksAPI.Data;
using BooksAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure connection string for the database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the DI container.
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    // Report the API versions supported by the API
    options.ReportApiVersions = true;
    // Default API version when version is not specified
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);

});



// Register services and repositories
builder.Services.AddSingleton<DapperContext>();
// Configure Dependency Injection for services and repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();

// Configure Dapper with SQL Server
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


// Enable Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Books Service", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Books Service", Version = "v2" });
});




var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books Service 1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Books Service 2");
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Books Service v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Books Service v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();