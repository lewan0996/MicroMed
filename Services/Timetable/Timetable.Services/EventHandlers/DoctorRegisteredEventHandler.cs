using Doctors.Contracts;
using MassTransit;
using Shared.Domain;
using Shared.Services;
using Timetable.Domain.DoctorAggregate;
using Timetable.Services.Repositories;

namespace Timetable.Services.EventHandlers;

public class DoctorRegisteredEventHandler : IConsumer<DoctorRegisteredEvent>
{
    private readonly IDoctorsRepository _doctorsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DoctorRegisteredEventHandler(IDoctorsRepository doctorsRepository, IUnitOfWork unitOfWork)
    {
        _doctorsRepository = doctorsRepository;
        _unitOfWork = unitOfWork;
    }

    public DoctorRegisteredEventHandler() { }

    public async Task Consume(ConsumeContext<DoctorRegisteredEvent> context)
    {
        var message = context.Message;

        var doctor = new Doctor(new Name(message.FirstName, message.LastName), Specialty.Get(message.SpecialtyId));

        await _doctorsRepository.AddDoctorAsync(doctor, context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}