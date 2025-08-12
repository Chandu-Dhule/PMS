using PMSCH.Server.Repositories;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories with proper DI
builder.Services.AddScoped<MachineRepository>();
builder.Services.AddScoped<MachineCategoryRepository>();
builder.Services.AddScoped<MachineTypeRepository>();
builder.Services.AddScoped<HealthMetricRepository>();
builder.Services.AddScoped<MaintenanceLogRepository>();

builder.Services.AddScoped<TechnicianMachineAssignmentRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    return new TechnicianMachineAssignmentRepository(connectionString);
});

builder.Services.AddScoped<UserRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var assignmentRepo = provider.GetRequiredService<TechnicianMachineAssignmentRepository>();
    return new UserRepository(config, assignmentRepo);
});

// Add role-based authorization policies (optional but recommended)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
    options.AddPolicy("TechnicianOnly", policy => policy.RequireRole("Technician"));
});

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
app.UseRouting();

// Custom Token Authentication Middleware
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["X-Custom-Token"].FirstOrDefault();

    if (!string.IsNullOrEmpty(token))
    {
        var userRepository = context.RequestServices.GetRequiredService<UserRepository>();

        if (userRepository.ValidateToken(token))
        {
            var user = userRepository.GetUserByToken(token);
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

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
