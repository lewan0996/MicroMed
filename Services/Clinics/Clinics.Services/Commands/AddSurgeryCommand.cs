using Clinics.Contracts;
using Clinics.Domain.ClinicAggregate;
using Clinics.Services.Repositories;
using MassTransit;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record AddSurgeryCommand(int ClinicId, SurgeryNumber Number, SurgeryFloor Floor, IReadOnlyList<int> EquipmentIds) : IRequest<int>
{
    public AddSurgeryCommand(int clinicId, string number, string floor, IReadOnlyList<int> equipmentIds)
        : this(clinicId, new SurgeryNumber(number), new SurgeryFloor(floor), equipmentIds) { }
}

public class AddSurgeryCommandHandler(
    IClinicRepository clinicRepository,
    IEquipmentRepository equipmentRepository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<AddSurgeryCommand, int>
{
    public async Task<int> Handle(AddSurgeryCommand request, CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetAsync(request.ClinicId, cancellationToken);

        var equipment = await equipmentRepository.GetEquipmentAsync(request.EquipmentIds, cancellationToken);

        var surgery = new Surgery(new SurgeryInfo(request.Number, request.Floor), equipment);

        clinic.AddSurgery(surgery);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(GetEvent(), cancellationToken: cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return surgery.Id;

        SurgeryAddedEvent GetEvent() => new(surgery.Id, surgery.SurgeryInfo.Floor.Value, surgery.SurgeryInfo.Number.Value);
    }
}