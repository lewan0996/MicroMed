using Clinics.Contracts;
using Clinics.Domain.ClinicAggregate;
using Clinics.Services.Repositories;
using MassTransit;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record AddSurgeryCommand(Guid ClinicId, SurgeryNumber Number, SurgeryFloor Floor, IReadOnlyList<Guid> EquipmentIds) : IRequest
{
    public AddSurgeryCommand(Guid clinicId, string number, string floor, IReadOnlyList<Guid> equipmentIds)
        : this(clinicId, new SurgeryNumber(number), new SurgeryFloor(floor), equipmentIds) { }
}

public class AddSurgeryCommandHandler : IRequestHandler<AddSurgeryCommand>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public AddSurgeryCommandHandler(IClinicRepository clinicRepository, IEquipmentRepository equipmentRepository,
        IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _clinicRepository = clinicRepository;
        _equipmentRepository = equipmentRepository;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(AddSurgeryCommand request, CancellationToken cancellationToken)
    {
        var clinic = await _clinicRepository.GetAsync(request.ClinicId, cancellationToken);

        var equipment = await _equipmentRepository.GetEquipmentAsync(request.EquipmentIds, cancellationToken);

        var surgery = new Surgery(new SurgeryInfo(request.Number, request.Floor), equipment);

        clinic.AddSurgery(surgery);

        await _publishEndpoint.Publish(GetEvent(), cancellationToken: cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        SurgeryAddedEvent GetEvent() => new(surgery.Id, surgery.SurgeryInfo.Floor.Value, surgery.SurgeryInfo.Number.Value);
    }
}