using Microsoft.EntityFrameworkCore;
using Shared.API;
using Shared.Services;
using Timetable.Infrastructure;
using Timetable.Infrastructure.Repositories;
using Timetable.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddOpenApi()
    .AddMediatRWithTransactionBehavior()
    .AddDbContext<TimetableDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        options.EnableSensitiveDataLogging();
    })
    .AddScoped<ISurgeryRepository, SurgeryRepository>()
    .AddScoped<IDoctorsRepository, DoctorsRepository>()
    .AddScoped<IAppointmentRepository, AppointmentRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<TimetableDbContext>())
    .AddMassTransit<TimetableDbContext>(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUIWithOpenApi();

    await EnsureDbCreated();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TimetableDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.Run();