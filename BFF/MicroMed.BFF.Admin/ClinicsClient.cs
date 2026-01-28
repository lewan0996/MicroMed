using Clinics.Contracts.Dto;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace MicroMed.BFF.Admin;

public class ClinicsClient
{
    private readonly string _clinicsServiceUrl;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClinicsClient(string clinicsServiceUrl, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _clinicsServiceUrl = clinicsServiceUrl;
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();
        
        if (_httpContextAccessor.HttpContext != null)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
        
        return httpClient;
    }

    public async Task<GetClinicsResponse> GetClinicsAsync(CancellationToken cancellationToken)
    {
        var httpClient = await CreateAuthenticatedClientAsync();
        return (await httpClient.GetFromJsonAsync<GetClinicsResponse>(_clinicsServiceUrl + "/Clinics", cancellationToken))!;
    }

    public async Task AddClinicAsync(AddClinicRequest request, CancellationToken cancellationToken = default)
    {
        var httpClient = await CreateAuthenticatedClientAsync();
        await httpClient.PostAsJsonAsync(_clinicsServiceUrl + "/Clinics", request, cancellationToken);
    }
}
