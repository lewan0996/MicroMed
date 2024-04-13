using Clinics.Contracts;
using Clinics.Domain.ClinicAggregate;
using Clinics.Services.Repositories;
using MassTransit;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record UpdateSurgeryCommand(int ClinicId, int SurgeryId, SurgeryInfo SurgeryInfo, IReadOnlyList<int> EquipmentIds) : IRequest
{
    public UpdateSurgeryCommand(int clinicId, int surgeryId, string number, string floor, IReadOnlyList<int> equipmentIds)
        : this(clinicId, surgeryId, new SurgeryInfo(new SurgeryNumber(number), new SurgeryFloor(floor)), equipmentIds) { }
}

public class UpdateSurgeryCommandHandler : IRequestHandler<UpdateSurgeryCommand>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateSurgeryCommandHandler(IClinicRepository clinicRepository, IEquipmentRepository equipmentRepository,
        IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _clinicRepository = clinicRepository;
        _equipmentRepository = equipmentRepository;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(UpdateSurgeryCommand request, CancellationToken cancellationToken)
    {
        var clinic = await _clinicRepository.GetAsync(request.ClinicId, cancellationToken);

        var equipment = await _equipmentRepository.GetEquipmentAsync(request.EquipmentIds, cancellationToken);

        clinic.UpdateSurgery(request.SurgeryId, request.SurgeryInfo, equipment);

        await _publishEndpoint.Publish(GetEvent(), cancellationToken: cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        SurgeryUpdatedEvent GetEvent() => new(request.SurgeryId, request.SurgeryInfo.Floor.Value, request.SurgeryInfo.Number.Value);
    }
}