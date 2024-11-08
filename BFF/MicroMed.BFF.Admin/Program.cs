using Clinics.Contracts;
using Doctors.Contracts;
using Grpc.Net.Client;
using MicroMed.BFF.Admin;
using MicroMed.BFF.Admin.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddGlobalExceptionHandler()
    .AddAuthorization()
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        var config = builder.Configuration.GetSection("Auth").Get<AuthConfig>()!;

        options.Authority = config.Authority;
        options.ClientId = "AdminPortal";
        options.ClientSecret = config.secret;
        options.ResponseType = "code";
        options.Scope.Add("admin");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.RequireHttpsMetadata = false;        
    });

builder.Services.AddBff();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app
    .UseDefaultFiles()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseBff()
    .UseAuthorization();

app.MapBffManagementEndpoints();

var serviceUrls = builder.Configuration.GetSection("Services").Get<ServiceUrls>()!;

var grpcOptions = CreateGrpcOptions();

app.MapGet("Clinics", async (CancellationToken cancellationToken) =>
{
    using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
    var client = new ClinicsService.ClinicsServiceClient(channel);

    var response = await client.GetClinicsAsync(new GetClinicsRequest(), cancellationToken: cancellationToken);

    return TypedResults.Ok(response.Clinics);
});

app.MapPost("Clinics",
        async (AddClinicRequest request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            var response = await client.AddClinicAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Created($"Clinics/{response.Id}");
        })
    .ProducesProblem(StatusCodes.Status400BadRequest);

app.MapPut("Clinics/{id:int}",
        async (int id, UpdateClinicDto request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            await client.UpdateClinicAsync(request.ToRpcRequest(id), cancellationToken: cancellationToken);

            return TypedResults.Ok();
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapPost("Clinics/{clinicId:int}/Surgeries",
        async (int clinicId, AddSurgeryDto request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            var response = await client.AddSurgeryAsync(request.ToRpcRequest(clinicId), cancellationToken: cancellationToken);

            return TypedResults.Created($"Clinics/{clinicId}/Surgeries/{response.Id}");
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapPut("Clinics/{clinicId:int}/Surgeries/{surgeryId:int}",
        async (int clinicId, int surgeryId, UpdateSurgeryDto request, CancellationToken cancellationToken) =>
        {
            using var channel = GrpcChannel.ForAddress(serviceUrls.Clinics, grpcOptions);
            var client = new ClinicsService.ClinicsServiceClient(channel);

            await client.UpdateSurgeryAsync(request.ToRpcRequest(clinicId, surgeryId), cancellationToken: cancellationToken);

            return TypedResults.Ok();
        })
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound);

app.MapDelete("Clinics/{clinicId:int}/Surgeries/{surgeryId:int}",
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

            var response = await client.AddSurgeryEquipmentAsync(request, cancellationToken: cancellationToken);

            return TypedResults.Created($"SurgeryEquipment/{response.Id}");
        })
    .ProducesProblem(StatusCodes.Status400BadRequest);

app.MapPost("Doctors",
    async (RegisterDoctorRequest request, CancellationToken cancellationToken) =>
    {
        using var channel = GrpcChannel.ForAddress(serviceUrls.Doctors, grpcOptions);
        var client = new DoctorsService.DoctorsServiceClient(channel);

        var response = await client.RegisterDoctorAsync(request, cancellationToken: cancellationToken);

        return TypedResults.Created($"Doctors/{response.Id}");
    }).ProducesProblem(StatusCodes.Status400BadRequest);

app.UseExceptionHandler();

app.Run();

return;

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

internal record ServiceUrls(string Clinics, string Doctors);

internal record AuthConfig(string Authority, string secret);