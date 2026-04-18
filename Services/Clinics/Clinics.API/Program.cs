using Clinics.API;
using Clinics.Infrastructure;
using Clinics.Infrastructure.Repositories;
using Clinics.Services.Repositories;
using Shared.API;
using Shared.Infrastructure;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IClinicRepository, ClinicRepository>()
    .AddScoped<IEquipmentRepository, EquipmentRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<ClinicsDbContext>())
    .AddMassTransit<ClinicsDbContext>(builder.Configuration)
    .AddPostgresConnectionProvider(builder.Configuration)
    .AddMediatRWithTransactionBehavior()
    .AddEfDbContextWithPostgres<ClinicsDbContext>(builder.Configuration)
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