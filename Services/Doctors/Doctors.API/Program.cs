using Doctors.API.Services;
using Doctors.Infrastructure;
using Doctors.Services;
using Microsoft.EntityFrameworkCore;
using Shared.API;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<RegisterDoctorCommand>())
    .AddDbContext<DoctorsDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        options.EnableSensitiveDataLogging();
    })
    .AddScoped<IDoctorsRepository, DoctorsRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<DoctorsDbContext>())
    .AddGrpcWithExceptionInterceptor();

builder.ConfigureNServiceBusEndpoint("Doctors");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await EnsureDbCreated();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DoctorsDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.UseHttpsRedirection();

app.MapGrpcService<DoctorsService>();

app.Run();