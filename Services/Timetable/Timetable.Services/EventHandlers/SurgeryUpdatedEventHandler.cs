using Clinics.Contracts;
using MassTransit;
using Shared.Services;
using Timetable.Services.Repositories;

namespace Timetable.Services.EventHandlers;

public class SurgeryUpdatedEventHandler : IConsumer<SurgeryUpdatedEvent>
{
    private readonly ISurgeryRepository _surgeryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SurgeryUpdatedEventHandler(ISurgeryRepository surgeryRepository, IUnitOfWork unitOfWork)
    {
        _surgeryRepository = surgeryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<SurgeryUpdatedEvent> context)
    {
        var message = context.Message;

        var surgery = await _surgeryRepository.GetAsync(message.Id, context.CancellationToken);

        surgery.Update(message.Floor, message.Number);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}