using Clinics.API;
using Grpc.Net.Client;
using MicroMed.Gateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddGlobalExceptionHandler();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var serviceUrls = new ServiceUrls();
builder.Configuration.Bind("Services", serviceUrls);

var grpcOptions = CreateGrpcOptions();

app.MapPost("Clinics",
        async (AddClinicRequest request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            var clinicId = await client.AddClinicAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Created($"Clinics/{clinicId}");
        })
    .ProducesProblem(StatusCodes.Status400BadRequest);

app.MapPut("Clinics",
        async (UpdateClinicRequest request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            await client.UpdateClinicAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Ok();
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapPost("Surgeries",
        async (AddSurgeryRequest request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            var surgeryId = await client.AddSurgeryAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Created($"Clinics/{request.ClinicId}/Surgeries/{surgeryId.Id}");
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapPut("Surgeries",
        async (UpdateSurgeryRequest request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            await client.UpdateSurgeryAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Ok();
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapDelete("Clinics/{clinicId}/Surgeries/{surgeryId}",
        async (int clinicId, int surgeryId, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            await client.RemoveSurgeryAsync(new RemoveSurgeryRequest { SurgeryId = surgeryId, ClinicId = clinicId },
                cancellationToken: cancellationToken);

            return TypedResults.Ok();
        })
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapPost("SurgeryEquipment",
        async (AddSurgeryEquipmentRequest request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            await client.AddSurgeryEquipmentAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Created();
        })
    .ProducesProblem(StatusCodes.Status400BadRequest);

app.MapPost("Doctors",
    async (RegisterDoctorRequest request, CancellationToken cancellationToken) =>
    {
        using var channel = GrpcChannel.ForAddress(serviceUrls.Doctors, grpcOptions);
        var client = new DoctorsService.DoctorsServiceClient(channel);

        var doctorId = await client.RegisterDoctorAsync(request, cancellationToken: cancellationToken);

        return TypedResults.Created($"Doctors/{doctorId.Id}"); //todo fix proto responses and requests
    }).ProducesProblem(StatusCodes.Status400BadRequest);

app.UseExceptionHandler();

app.Run();

GrpcChannelOptions CreateGrpcOptions()
{
    var unsafeHttpClientHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    var grpcChannelOptions = new GrpcChannelOptions();

    if (app.Environment.IsDevelopment())
    {
        grpcChannelOptions.HttpHandler = unsafeHttpClientHandler;
    }

    return grpcChannelOptions;
}

class ServiceUrls
{
    public string Clinics { get; set; }
    public string Doctors { get; set; }
}