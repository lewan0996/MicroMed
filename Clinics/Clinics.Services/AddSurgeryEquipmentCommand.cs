using Clinics.Domain.EquipmentAggregate;
using Clinics.Services.Repositories;
using MediatR;
using Shared.Services;

namespace Clinics.Services;

public record AddSurgeryEquipmentCommand(EquipmentName Name) : IRequest
{
    public AddSurgeryEquipmentCommand(string name) : this(new EquipmentName(name)) { }
}

public class AddSurgeryEquipmentCommandHandler : IRequestHandler<AddSurgeryEquipmentCommand>
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddSurgeryEquipmentCommandHandler(IEquipmentRepository equipmentRepository, IUnitOfWork unitOfWork)
    {
        _equipmentRepository = equipmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddSurgeryEquipmentCommand request, CancellationToken cancellationToken)
    {
        var equipment = new Equipment(request.Name);

        await _equipmentRepository.AddAsync(equipment, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}