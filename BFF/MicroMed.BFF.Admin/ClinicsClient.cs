using Clinics.Contracts.Dto;

namespace MicroMed.BFF.Admin;

public class ClinicsClient
{
    private readonly string _clinicsServiceUrl;
    private readonly HttpClient _httpClient;

    public ClinicsClient(string clinicsServiceUrl, IHttpClientFactory httpClientFactory)
    {
        _clinicsServiceUrl = clinicsServiceUrl;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<GetClinicsResponse> GetClinicsAsync(CancellationToken cancellationToken) 
        => (await _httpClient.GetFromJsonAsync<GetClinicsResponse>(_clinicsServiceUrl + "/Clinics", cancellationToken))!;

    public Task AddClinicAsync(AddClinicRequest request, CancellationToken cancellationToken = default) 
        => _httpClient.PostAsJsonAsync(_clinicsServiceUrl + "/Clinics", request, cancellationToken);
}
