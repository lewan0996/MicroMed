using Grpc.Net.Client;
using MicroMed.Gateway;
using Shared.API;

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

            var result = await client.AddClinicAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Created($"Clinics/{result}");
        })
    .ProducesProblem(StatusCodes.Status400BadRequest);

app.MapPost("Surgeries",
        async (AddSurgeryRequest request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            await client.AddSurgeryAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Created();
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapPost("SurgeryEquipment",
        async (AddSurgeryEquipmentRequest request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            await client.AddSurgeryEquipmentAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Created();
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapPost("Doctors",
    async (RegisterDoctorRequest request, CancellationToken cancellationToken) =>
    {
        using var channel = GrpcChannel.ForAddress(serviceUrls.Doctors, grpcOptions);
        var client = new DoctorsService.DoctorsServiceClient(channel);

        await client.RegisterDoctorAsync(request, cancellationToken: cancellationToken);

        return TypedResults.Created();
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