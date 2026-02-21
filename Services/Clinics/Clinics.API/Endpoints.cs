using Clinics.Contracts.Dto;
using Clinics.Services.Commands;
using Clinics.Services.Queries;
using MediatR;

namespace Clinics.API;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("Clinics", (IMediator mediator, CancellationToken cancellationToken)
                => mediator.Send(new ClinicsQuery(), cancellationToken))
            .RequireAuthorization();

        app.MapPost("Clinics", async (IMediator mediator, AddClinicRequest request, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new AddClinicCommand(request), cancellationToken);
            return Results.Ok(new AddClinicResponse(result)); // todo change to Created
        });

        app.MapPut("Clinics/{clinicId:int}", async (IMediator mediator, int clinicId, UpdateClinicRequest request) =>
        {
            await mediator.Send(new UpdateClinicCommand(clinicId, request));
            return Results.NoContent();
        });

        app.MapPost("Clinics/{clinicId:int}/Surgeries",
            async (IMediator mediator, int clinicId, AddSurgeryRequest request, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new AddSurgeryCommand(clinicId, request), cancellationToken);
                return Results.Ok(new AddSurgeryResponse(result)); // todo change to Created
            });

        app.MapPut("Clinics/{clinicId:int}/Surgeries/{surgeryId:int}",
            async (IMediator mediator, int clinicId, int surgeryId, UpdateSurgeryRequest request,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new UpdateSurgeryCommand(clinicId, surgeryId, request), cancellationToken);
                return Results.NoContent();
            });

        app.MapDelete("Clinics/{clinicId:int}/Surgeries/{surgeryId:int}",
            async (IMediator mediator, int clinicId, int surgeryId, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new RemoveSurgeryCommand(clinicId, surgeryId), cancellationToken);
                return Results.NoContent();
            });

        app.MapPost("SurgeryEquipment",
            async (IMediator mediator, AddSurgeryEquipmentRequest request, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new AddSurgeryEquipmentCommand(request), cancellationToken);
                return Results.Ok(new AddSurgeryEquipmentResponse(result)); // todo change to Created
            });
    }
}