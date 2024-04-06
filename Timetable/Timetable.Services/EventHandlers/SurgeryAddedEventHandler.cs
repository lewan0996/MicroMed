using Clinics.Contracts;
using NServiceBus;
using Shared.Services;
using Timetable.Domain.SurgeryAggregate;
using Timetable.Services.Repositories;

namespace Timetable.Services.EventHandlers;

public class SurgeryAddedEventHandler : IHandleMessages<SurgeryAddedEvent>
{
    private readonly ISurgeryRepository _surgeryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SurgeryAddedEventHandler(ISurgeryRepository surgeryRepository, IUnitOfWork unitOfWork)
    {
        _surgeryRepository = surgeryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SurgeryAddedEvent message, IMessageHandlerContext context)
    {
        var surgery = new Surgery(message.Floor, message.Number);

        await _surgeryRepository.AddSurgeryAsync(surgery, context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}