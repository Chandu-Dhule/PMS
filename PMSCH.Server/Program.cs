using PMSCH.Server.Repositories;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories
builder.Services.AddScoped<MachineRepository>();
builder.Services.AddScoped<MachineCategoryRepository>();
builder.Services.AddScoped<MachineTypeRepository>();
builder.Services.AddScoped<HealthMetricRepository>();
builder.Services.AddScoped<MaintenanceLogRepository>();
builder.Services.AddScoped<UserRepository>(); // For custom token auth

// Add authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

// Serve static files
app.UseDefaultFiles();
app.UseStaticFiles();

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS redirection
app.UseHttpsRedirection();

// Custom Token Authentication Middleware
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["X-Custom-Token"].FirstOrDefault();

    if (!string.IsNullOrEmpty(token))
    {
        var userRepository = context.RequestServices.GetRequiredService<UserRepository>();

        if (userRepository.ValidateToken(token))
        {
            // Assuming token is unique and maps to one user
            var user = userRepository.GetUserByToken(token); // You can implement this method

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, "Custom");
                context.User = new ClaimsPrincipal(identity);
            }
        }
    }

    await next();
});

// Authorization
app.UseAuthorization();

// Map controllers and fallback
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
