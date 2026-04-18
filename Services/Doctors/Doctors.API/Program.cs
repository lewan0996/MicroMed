using Doctors.API;
using Doctors.Infrastructure;
using Doctors.Services;
using Shared.API;
using Shared.Infrastructure;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IDoctorsRepository, DoctorsRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<DoctorsDbContext>())
    .AddMassTransit<DoctorsDbContext>(builder.Configuration)
    .AddPostgresConnectionProvider(builder.Configuration)
    .AddMediatRWithTransactionBehavior()
    .AddEfDbContextWithPostgres<DoctorsDbContext>(builder.Configuration)
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