using Clinics.Services.Commands;
using Grpc.Core;
using MediatR;

namespace Clinics.API.Services;

public class ClinicsService : API.ClinicsService.ClinicsServiceBase
{
    private readonly IMediator _mediator;

    public ClinicsService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<AddClinicResponse> AddClinic(AddClinicRequest request, ServerCallContext context)
    {
        var result =
            await _mediator.Send(
                new AddClinicCommand(request.Name, request.City, request.Street, request.StreetNumber,
                    request.AdditionalInfo), context.CancellationToken);

        return new AddClinicResponse { Id = result };
    }

    public override async Task<UpdateClinicResponse> UpdateClinic(UpdateClinicRequest request, ServerCallContext context)
    {
        await _mediator.Send(
                new UpdateClinicCommand(request.ClinicId, request.Name, request.City, request.Street, request.StreetNumber,
                    request.AdditionalInfo), context.CancellationToken);

        return new UpdateClinicResponse();
    }

    public override async Task<AddSurgeryResponse> AddSurgery(AddSurgeryRequest request, ServerCallContext context)
    {
        await _mediator.Send(
            new AddSurgeryCommand(request.ClinicId, request.Number, request.Floor, request.EquipmentIds),
            context.CancellationToken);

        return new AddSurgeryResponse();
    }

    public override async Task<UpdateSurgeryResponse> UpdateSurgery(UpdateSurgeryRequest request, ServerCallContext context)
    {
        await _mediator.Send(
            new UpdateSurgeryCommand(request.ClinicId, request.SurgeryId, request.Number, request.Floor, request.EquipmentIds),
            context.CancellationToken);

        return new UpdateSurgeryResponse();
    }

    public override async Task<RemoveSurgeryResponse> RemoveSurgery(RemoveSurgeryRequest request, ServerCallContext context)
    {
        await _mediator.Send(
            new RemoveSurgeryCommand(request.ClinicId, request.SurgeryId),
            context.CancellationToken);

        return new RemoveSurgeryResponse();
    }

    public override async Task<AddSurgeryEquipmentResponse> AddSurgeryEquipment(AddSurgeryEquipmentRequest request, ServerCallContext context)
    {
        await _mediator.Send(new AddSurgeryEquipmentCommand(request.Name), context.CancellationToken);

        return new AddSurgeryEquipmentResponse();
    }
}