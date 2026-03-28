using Doctors.Contracts;
using MassTransit;
using Shared.Domain;
using Shared.Services;
using Timetable.Domain.DoctorAggregate;
using Timetable.Services.Repositories;

namespace Timetable.Services.EventHandlers;

public class DoctorRegisteredEventHandler(IDoctorsRepository doctorsRepository, IUnitOfWork unitOfWork)
    : IConsumer<DoctorRegisteredEvent>
{
    public async Task Consume(ConsumeContext<DoctorRegisteredEvent> context)
    {
        var message = context.Message;

        var doctor = new Doctor(message.Id, new Name(message.FirstName, message.LastName), Specialty.Get(message.SpecialtyId));

        await doctorsRepository.AddDoctorAsync(doctor, context.CancellationToken);

        await unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}