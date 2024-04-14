using Clinics.Contracts.Events;
using MassTransit;
using Shared.Services;
using Timetable.Services.Repositories;

namespace Timetable.Services.EventHandlers;

public class SurgeryRemovedEventHandler : IConsumer<SurgeryRemovedEvent>
{
    private readonly ISurgeryRepository _surgeryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SurgeryRemovedEventHandler(ISurgeryRepository surgeryRepository, IUnitOfWork unitOfWork)
    {
        _surgeryRepository = surgeryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<SurgeryRemovedEvent> context)
    {
        var message = context.Message;

        await _surgeryRepository.RemoveAsync(message.Id, context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}