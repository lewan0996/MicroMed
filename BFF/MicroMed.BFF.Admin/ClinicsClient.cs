using Clinics.Contracts;
using Grpc.Core;
using Grpc.Net.Client;
using static Clinics.Contracts.GetClinicsResponse.Types;

namespace MicroMed.BFF.Admin;

public class ClinicsClient
{
    private readonly GrpcChannelOptions _grpcChannelOptions;
    private readonly string _clinicsServiceUrl;

    public ClinicsClient(bool isDevelopment, string clinicsServiceUrl)
    {
        _grpcChannelOptions = CreateGrpcOptions(isDevelopment);
        _clinicsServiceUrl = clinicsServiceUrl;
    }

    public async Task<ClinicDto[]> GetClinicsAsync(CancellationToken cancellationToken)
    {
        var response = await CallClient(c => c.GetClinicsAsync(new GetClinicsRequest(), cancellationToken: cancellationToken));

        return [.. response.Clinics];
    }

    private async Task<TResult> CallClient<TResult>(Func<ClinicsService.ClinicsServiceClient, AsyncUnaryCall<TResult>> task)
    {
        using var channel = GrpcChannel.ForAddress(_clinicsServiceUrl, _grpcChannelOptions);
        var client = new ClinicsService.ClinicsServiceClient(channel);

        return await task(client);
    }

    private GrpcChannelOptions CreateGrpcOptions(bool isDevelopment)
    {
        var unsafeHttpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var grpcChannelOptions = new GrpcChannelOptions();

        if (isDevelopment)
        {
            grpcChannelOptions.HttpHandler = unsafeHttpClientHandler;
        }

        return grpcChannelOptions;
    }

    internal record ServiceUrls(string Clinics, string Doctors);
}
