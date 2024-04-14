using Clinics.API.Services;
using Clinics.Infrastructure;
using Clinics.Infrastructure.Repositories;
using Clinics.Services.Repositories;
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
    .AddScoped<IClinicRepository, ClinicRepository>()
    .AddScoped<IEquipmentRepository, EquipmentRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<ClinicsDbContext>())
    .AddMassTransit<ClinicsDbContext>(builder.Configuration)
    .AddGrpcWithExceptionInterceptor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await EnsureDbCreated();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ClinicsDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.UseHttpsRedirection();

app.MapGrpcService<ClinicsService>();

app.Run();