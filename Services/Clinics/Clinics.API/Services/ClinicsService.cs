using Clinics.Contracts;
using Clinics.Services.Commands;
using Clinics.Services.Queries;
using Grpc.Core;
using MediatR;

namespace Clinics.API.Services;

public class ClinicsService(IMediator mediator) : Contracts.ClinicsService.ClinicsServiceBase
{
    public override Task<GetClinicsResponse> GetClinics(GetClinicsRequest request, ServerCallContext context)
        => mediator.Send(new ClinicsQuery());

    public override async Task<AddClinicResponse> AddClinic(AddClinicRequest request, ServerCallContext context)
    {
        var result =
            await mediator.Send(
                new AddClinicCommand(request.Name, request.City, request.Street, request.StreetNumber,
                    request.AdditionalInfo), context.CancellationToken);

        return new AddClinicResponse { Id = result };
    }

    public override async Task<UpdateClinicResponse> UpdateClinic(UpdateClinicRequest request, ServerCallContext context)
    {
        await mediator.Send(
                new UpdateClinicCommand(request.ClinicId, request.Name, request.City, request.Street, request.StreetNumber,
                    request.AdditionalInfo), context.CancellationToken);

        return new UpdateClinicResponse();
    }

    public override async Task<AddSurgeryResponse> AddSurgery(AddSurgeryRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(
            new AddSurgeryCommand(request.ClinicId, request.Number, request.Floor, request.EquipmentIds),
            context.CancellationToken);

        return new AddSurgeryResponse { Id = result };
    }

    public override async Task<UpdateSurgeryResponse> UpdateSurgery(UpdateSurgeryRequest request, ServerCallContext context)
    {
        await mediator.Send(
            new UpdateSurgeryCommand(request.ClinicId, request.SurgeryId, request.Number, request.Floor, request.EquipmentIds),
            context.CancellationToken);

        return new UpdateSurgeryResponse();
    }

    public override async Task<RemoveSurgeryResponse> RemoveSurgery(RemoveSurgeryRequest request, ServerCallContext context)
    {
        await mediator.Send(
            new RemoveSurgeryCommand(request.ClinicId, request.SurgeryId),
            context.CancellationToken);

        return new RemoveSurgeryResponse();
    }

    public override async Task<AddSurgeryEquipmentResponse> AddSurgeryEquipment(AddSurgeryEquipmentRequest request, ServerCallContext context)
    {
        var result = await mediator.Send(new AddSurgeryEquipmentCommand(request.Name), context.CancellationToken);

        return new AddSurgeryEquipmentResponse { Id = result };
    }
}