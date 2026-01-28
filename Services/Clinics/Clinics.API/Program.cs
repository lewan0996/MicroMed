using Clinics.API;
using Clinics.Infrastructure;
using Clinics.Infrastructure.Repositories;
using Clinics.Services.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Shared.API;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMediatRWithTransactionBehavior()
    .AddDbContext<ClinicsDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        options.EnableSensitiveDataLogging();
    })
    .AddEndpointsApiExplorer()
    .AddOpenApi()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authSettings = builder.Configuration.GetSection("Authentication");
        options.Authority = authSettings["Authority"];
        options.Audience = "clinics.api";
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
    .AddScoped<IClinicRepository, ClinicRepository>()
    .AddScoped<IEquipmentRepository, EquipmentRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<ClinicsDbContext>())
    .AddMassTransit<ClinicsDbContext>(builder.Configuration)
    .AddSqlConnectionProvider(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await EnsureDbCreated();

    app.UseSwaggerUIWithOpenApi();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ClinicsDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();