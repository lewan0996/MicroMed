using Doctors.Contracts.Dto;
using Doctors.Services;
using MediatR;

namespace Doctors.API;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapPost("Doctors", async (IMediator mediator, RegisterDoctorDto dto, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new RegisterDoctorCommand(dto), cancellationToken);
            return Results.Ok(new RegisterDoctorResponse(result)); // todo: change to Created
        }).RequireAuthorization();
    }
}
