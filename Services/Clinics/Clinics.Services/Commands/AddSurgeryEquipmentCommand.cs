using Clinics.Contracts.Dto;
using Clinics.Domain.EquipmentAggregate;
using Clinics.Services.Repositories;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record AddSurgeryEquipmentCommand(EquipmentName Name) : IRequest<Guid>
{
    public AddSurgeryEquipmentCommand(string name) : this(new EquipmentName(name)) { }
    
    public AddSurgeryEquipmentCommand(AddSurgeryEquipmentRequest request) : this(new EquipmentName(request.Name)) { }
}

public class AddSurgeryEquipmentCommandHandler(IEquipmentRepository equipmentRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AddSurgeryEquipmentCommand, Guid>
{
    public async Task<Guid> Handle(AddSurgeryEquipmentCommand request, CancellationToken cancellationToken)
    {
        var equipment = new Equipment(request.Name);

        await equipmentRepository.AddAsync(equipment, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return equipment.Id;
    }
}