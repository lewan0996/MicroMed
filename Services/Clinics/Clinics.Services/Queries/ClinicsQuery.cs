using Clinics.Contracts.Dto;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Queries;

public class ClinicsQuery : IRequest<GetClinicsResponse>;

public class ClinicsQueryHandler(PostgresConnectionProvider sqlConnectionProvider) : IRequestHandler<ClinicsQuery, GetClinicsResponse>
{
    public async Task<GetClinicsResponse> Handle(ClinicsQuery request, CancellationToken cancellationToken)
    {
        const string sql = $"""

                            SELECT
                                id,
                                name,
                                city,
                                street,
                                street_number,
                                additional_info
                            FROM {Tables.Clinics}
                            """;

        var clinics = await sqlConnectionProvider.CallAsync(x => x.QueryAsync<ClinicDto>(sql, cancellationToken));

        var result = new GetClinicsResponse(clinics.ToList());

        return result;
    }
}