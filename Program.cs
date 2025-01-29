using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger configuration
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Manager API",
        Version = "v1",
        Description = "API for managing tasks with CRUD operations, pagination, and search."
    });

     // Include OpenAPI YAML file from task-manager-docs directory
    var yamlPath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory)?.FullName ?? string.Empty, @"..\task-manager-docs\task-manager-api-v1.0.0.yaml");
    if (File.Exists(yamlPath))
    {
        c.IncludeXmlComments(yamlPath); // Attach the YAML file
    }
});

// Configure Entity Framework Core with In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MockDatabase"));

// Configure CORS to allow any origin, method, and header
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed the in-memory database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    TaskSeeder.SeedTasks(dbContext); // Seed mock data into the in-memory database
}

// Enable the CORS policy
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    // Enable Swagger and Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API v1");
        c.RoutePrefix = string.Empty; // Access Swagger UI at the root URL
    });
}

// Middleware for handling global errors
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new
        {
            error = ex.Message
        });
    }
});

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseAuthorization();    // Handle Authorization
app.MapControllers();      // Map API controllers

app.Run();
