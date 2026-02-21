using Doctors.API;
using Doctors.Infrastructure;
using Doctors.Services;
using Microsoft.EntityFrameworkCore;
using Shared.API;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IDoctorsRepository, DoctorsRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<DoctorsDbContext>())
    .AddMassTransit<DoctorsDbContext>(builder.Configuration)
    .AddSqlConnectionProvider(builder.Configuration)
    .AddMediatRWithTransactionBehavior()
    .AddDbContext<DoctorsDbContext>(options =>
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

        var dbContext = scope.ServiceProvider.GetRequiredService<DoctorsDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.UseSwaggerUI();
app.MapEndpoints();

app
    .UseAuthentication()
    .UseAuthorization();

app.Run();