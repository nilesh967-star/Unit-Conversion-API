using UnitConversionApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// CORS — allow the local React dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Register the conversion service
builder.Services.AddScoped<IConversionService, ConversionService>();

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title       = "Unit Conversion API",
        Version     = "v1",
        Description = "Convert numerical values between units of measurement (length, weight, temperature, volume, speed, area, energy)."
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Unit Conversion API v1");
    c.RoutePrefix = string.Empty; // Swagger UI at root
});

app.UseCors("FrontendDev");
app.UseAuthorization();
app.MapControllers();

app.Run();

// Expose for integration tests
public partial class Program { }
