using Clinics.Contracts;
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
            => mediator.Send(new ClinicsQuery(), cancellationToken));

        app.MapPost("Clinics", async (IMediator mediator, AddClinicDto dto, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new AddClinicCommand(dto), cancellationToken);
            return Results.Ok(new AddClinicResponse { Id = result }); // todo change to Created
        });

        app.MapPut("Clinics/{clinicId:int}", async (IMediator mediator, int clinicId, UpdateClinicDto dto) =>
        {
            await mediator.Send(new UpdateClinicCommand(clinicId, dto));
            return Results.NoContent();
        });

        app.MapPost("Clinics/{clinicId:int}/Surgeries",
            async (IMediator mediator, int clinicId, AddSurgeryDto dto, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new AddSurgeryCommand(clinicId, dto), cancellationToken);
                return Results.Ok(new AddSurgeryResponse { Id = result }); // todo change to Created
            });

        app.MapPut("Clinics/{clinicId:int}/Surgeries/{surgeryId:int}",
            async (IMediator mediator, int clinicId, int surgeryId, UpdateSurgeryDto dto,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new UpdateSurgeryCommand(clinicId, surgeryId, dto), cancellationToken);
                return Results.NoContent();
            });

        app.MapDelete("Clinics/{clinicId:int}/Surgeries/{surgeryId:int}",
            async (IMediator mediator, int clinicId, int surgeryId, CancellationToken cancellationToken) =>
            {
                await mediator.Send(new RemoveSurgeryCommand(clinicId, surgeryId), cancellationToken);
                return Results.NoContent();
            });

        app.MapPost("SurgeryEquipment",
            async (IMediator mediator, AddSurgeryEquipmentDto dto, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new AddSurgeryEquipmentCommand(dto), cancellationToken);
                return Results.Ok(new AddSurgeryEquipmentResponse { Id = result }); // todo change to Created
            });
    }
}