using Clinics.Infrastructure;
using Clinics.Infrastructure.Repositories;
using Clinics.Services;
using Clinics.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.API;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<AddClinicCommand>())
    .AddDbContext<ClinicsDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        options.EnableSensitiveDataLogging();
    })
    .AddScoped<IClinicRepository, ClinicRepository>()
    .AddScoped<IEquipmentRepository, EquipmentRepository>()
    .AddScoped<IUnitOfWork>(services => services.GetRequiredService<ClinicsDbContext>())
    .AddGlobalExceptionHandler();

builder.ConfigureNServiceBusEndpoint("Clinics");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await EnsureDbCreated();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ClinicsDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapPost("Clinics",
        async (AddClinicDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new AddClinicCommand(dto.Name, dto.City, dto.Street, dto.StreetNumber, dto.AdditionalInfo));
            return TypedResults.Created($"Clinics/{result}");
        })
    .ProducesProblem(StatusCodes.Status400BadRequest);

app.MapPost("Surgeries",
        async (AddSurgeryDto dto, IMediator mediator) =>
        {
            await mediator.Send(new AddSurgeryCommand(dto.ClinicId, dto.Number, dto.Floor, dto.EquipmentIds));
            return TypedResults.Created();
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapPost("SurgeryEquipment",
        async (AddSurgeryEquipmentDto dto, IMediator mediator) =>
        {
            await mediator.Send(new AddSurgeryEquipmentCommand(dto.Name));
            return TypedResults.Created();
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.Run();

public record AddClinicDto(string Name, string City, string Street, string StreetNumber, string AdditionalInfo);
public record AddSurgeryDto(int ClinicId, string Number, string Floor, List<int> EquipmentIds);
public record AddSurgeryEquipmentDto(string Name);