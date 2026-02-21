using Microsoft.EntityFrameworkCore;
using Shared.API;
using Shared.Services;
using Timetable.API;
using Timetable.Infrastructure;
using Timetable.Infrastructure.Repositories;
using Timetable.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<ISurgeryRepository, SurgeryRepository>()
    .AddScoped<IDoctorsRepository, DoctorsRepository>()
    .AddScoped<IAppointmentRepository, AppointmentRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<TimetableDbContext>())
    .AddMassTransit<TimetableDbContext>(builder.Configuration)
    .AddSqlConnectionProvider(builder.Configuration)
    .AddMediatRWithTransactionBehavior()
    .AddDbContext<TimetableDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        options.EnableSensitiveDataLogging();
    })
    .AddEndpointsApiExplorer()
    .AddSwagger(builder.Configuration, builder.Environment)
    .AddAuth(builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await EnsureDbCreated();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TimetableDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.UseSwaggerUI();
app.MapEndpoints();

app
    .UseAuthentication()
    .UseAuthorization();

app.Run();