using Doctors.API.Contracts;
using Doctors.Infrastructure;
using Doctors.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.API;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<RegisterDoctorCommand>())
    .AddDbContext<DoctorsDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        options.EnableSensitiveDataLogging();
    })
    .AddScoped<IDoctorsRepository, DoctorsRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<DoctorsDbContext>())
    .AddGlobalExceptionHandler();

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

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapPost("Doctors",
    async (RegisterDoctorDto dto, IMediator mediator) =>
    {
        await mediator.Send(new RegisterDoctorCommand(dto.FirstName, dto.LastName, dto.SpecialtyId));
        return TypedResults.Created();
    }).ProducesProblem(StatusCodes.Status400BadRequest);

app.Run();