using Doctors.Contracts;
using NServiceBus;

namespace Timetable.Services;

public class DoctorRegisteredEventHandler : IHandleMessages<DoctorRegisteredEvent>
{
    public Task Handle(DoctorRegisteredEvent message, IMessageHandlerContext context)
    {
        throw new NotImplementedException();
    }
}