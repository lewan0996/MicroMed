using Clinics.Contracts;
using MassTransit;
using Shared.Services;
using Timetable.Domain.SurgeryAggregate;
using Timetable.Services.Repositories;

namespace Timetable.Services.EventHandlers;

public class SurgeryAddedEventHandler : IConsumer<SurgeryAddedEvent>
{
    private readonly ISurgeryRepository _surgeryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SurgeryAddedEventHandler(ISurgeryRepository surgeryRepository, IUnitOfWork unitOfWork)
    {
        _surgeryRepository = surgeryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<SurgeryAddedEvent> context)
    {
        var message = context.Message;

        var surgery = new Surgery(message.Id, message.Floor, message.Number);

        await _surgeryRepository.AddSurgeryAsync(surgery, context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}