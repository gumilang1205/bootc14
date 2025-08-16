using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Repositories;
using StudentManagement.Service;
using StudentManagement.Mapping;
using FluentValidation.AspNetCore;
using StudentManagement.Validator;
using StudentManagement.DTOs;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework with SQLite
// In a real application, you would store the connection string in appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=ProductCatalog.db"));

// Register services for dependency injection
// This follows the dependency inversion principle - controllers depend on abstractions, not concrete implementations
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddAutoMapper(typeof(StudentMappingProfile));
builder.Services.AddTransient<IValidator<StudentCreateDto>, StudentValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Add controllers with JSON options configuration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization to handle reference loops
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        // Use camelCase for property names in JSON responses
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Configure API documentation with Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Product Catalog API",
        Version = "v1",
        Description = "A comprehensive ASP.NET Core Web API demonstrating REST principles, Entity Framework, and clean architecture patterns."
    });
});

// Configure CORS for web client integration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ensure database is created and seeded on startup
// In production, you would use migrations through deployment pipelines
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enable Swagger only in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at app's root
    });
}

// Enable CORS
app.UseCors("AllowAll");

// Enable HTTPS redirection for security
app.UseHttpsRedirection();

// Enable authorization (even though we're not using authentication in this demo)
app.UseAuthorization();

// Map controller routes
app.MapControllers();

app.Run();