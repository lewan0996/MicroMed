using Doctors.API;
using Doctors.Infrastructure;
using Doctors.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Shared.API;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMediatRWithTransactionBehavior()
    .AddDbContext<DoctorsDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        options.EnableSensitiveDataLogging();
    })
    .AddScoped<IDoctorsRepository, DoctorsRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<DoctorsDbContext>())
    .AddMassTransit<DoctorsDbContext>(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddOpenApi()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authSettings = builder.Configuration.GetSection("Authentication");
        options.Authority = authSettings["Authority"];
        options.Audience = "doctors.api";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true
        };
    });

builder.Services
    .AddAuthorization()
    .AddSqlConnectionProvider(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await EnsureDbCreated();

    app.UseSwaggerUIWithOpenApi();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DoctorsDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();