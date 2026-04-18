using Shared.API;
using Shared.Infrastructure;
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
    .AddPostgresConnectionProvider(builder.Configuration)
    .AddMediatRWithTransactionBehavior()
    .AddEfDbContextWithPostgres<TimetableDbContext>(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwagger(builder.Configuration, builder.Environment)
    .AddAuth(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseSwaggerUI();
app.MapEndpoints();

app
    .UseAuthentication()
    .UseAuthorization();

app.Run();