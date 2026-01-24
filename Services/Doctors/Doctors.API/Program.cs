using Doctors.API;
using Doctors.Infrastructure;
using Doctors.Services;
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
    .AddSwaggerGen()
    .AddSqlConnectionProvider(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await EnsureDbCreated();

    app.UseSwagger();
    app.UseSwaggerUI();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DoctorsDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.MapEndpoints();

app.Run();