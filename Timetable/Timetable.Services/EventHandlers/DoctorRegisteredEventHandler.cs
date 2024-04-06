using Doctors.Contracts;
using NServiceBus;
using Shared.Domain;
using Shared.Services;
using Timetable.Domain.DoctorAggregate;
using Timetable.Services.Repositories;

namespace Timetable.Services.EventHandlers;

public class DoctorRegisteredEventHandler : IHandleMessages<DoctorRegisteredEvent>
{
    private readonly IDoctorsRepository _doctorsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DoctorRegisteredEventHandler(IDoctorsRepository doctorsRepository, IUnitOfWork unitOfWork)
    {
        _doctorsRepository = doctorsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DoctorRegisteredEvent message, IMessageHandlerContext context)
    {
        var doctor = new Doctor(new Name(message.FirstName, message.LastName), Specialty.Get(message.SpecialtyId));

        await _doctorsRepository.AddDoctorAsync(doctor, context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}