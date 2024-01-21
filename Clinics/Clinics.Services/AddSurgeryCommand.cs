using Clinics.Domain.ClinicAggregate;
using Clinics.Services.Repositories;
using MediatR;
using Shared.Services;

namespace Clinics.Services;

public record AddSurgeryCommand(int ClinicId, SurgeryNumber Number, SurgeryFloor Floor, IReadOnlyList<int> EquipmentIds) : IRequest
{
    public AddSurgeryCommand(int clinicId, string number, string floor, IReadOnlyList<int> equipmentIds) 
        : this(clinicId, new SurgeryNumber(number), new SurgeryFloor(floor), equipmentIds) { }
}

public class AddSurgeryCommandHandler : IRequestHandler<AddSurgeryCommand>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddSurgeryCommandHandler(IClinicRepository clinicRepository, IEquipmentRepository equipmentRepository, IUnitOfWork unitOfWork)
    {
        _clinicRepository = clinicRepository;
        _equipmentRepository = equipmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddSurgeryCommand request, CancellationToken cancellationToken)
    {
        var clinic = await _clinicRepository.GetAsync(request.ClinicId, cancellationToken);

        var equipment = await _equipmentRepository.GetEquipmentAsync(request.EquipmentIds, cancellationToken);

        var surgery = new Surgery(request.Number, request.Floor, equipment);

        clinic.AddSurgery(surgery);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}