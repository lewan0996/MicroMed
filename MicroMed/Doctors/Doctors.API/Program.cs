using Doctors.API.Contracts;
using Doctors.Infrastructure;
using Doctors.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<RegisterDoctorCommand>());
builder.Services.AddDbContext<DoctorsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    options.EnableSensitiveDataLogging();
});
builder.Services.AddScoped<IDoctorsRepository, DoctorsRepository>();
builder.Services.AddScoped<IUnitOfWork>(services => services.GetRequiredService<DoctorsDbContext>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await EnsureDbCreated();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DoctorsDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.UseHttpsRedirection();

app.MapPost("Doctors",
    async (RegisterDoctorDto dto, IMediator mediator) =>
    {
        await mediator.Send(new RegisterDoctorCommand(dto.FirstName, dto.LastName, dto.Specialty));
        return TypedResults.Created();
    });

app.Run();