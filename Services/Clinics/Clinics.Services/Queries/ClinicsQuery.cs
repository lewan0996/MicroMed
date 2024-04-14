using Clinics.Contracts;
using MediatR;
using Shared.Services;
using ClinicDto = Clinics.Contracts.GetClinicsResponse.Types.ClinicDto;

namespace Clinics.Services.Queries;

public class ClinicsQuery : IRequest<GetClinicsResponse>;

public class ClinicsQueryHandler(SqlConnectionProvider sqlConnectionProvider) : IRequestHandler<ClinicsQuery, GetClinicsResponse>
{
    public async Task<GetClinicsResponse> Handle(ClinicsQuery request, CancellationToken cancellationToken)
    {
        const string sql = $@"
SELECT
    Id,
    Name,
    City,
    Street,
    StreetNumber,
    AdditionalInfo
FROM {Tables.Clinics}";

        var clinics = await sqlConnectionProvider.CallAsync(x => x.QueryAsync<ClinicDto>(sql, cancellationToken));

        var result = new GetClinicsResponse();
        result.Clinics.AddRange(clinics);

        return result;
    }
}