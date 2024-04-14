using Doctors.Contracts;
using Doctors.Services;
using Grpc.Core;
using MediatR;

namespace Doctors.API.Services;

public class DoctorsService(IMediator mediator) : Contracts.DoctorsService.DoctorsServiceBase
{
    public override async Task<RegisterDoctorResponse> RegisterDoctor(RegisterDoctorRequest request, ServerCallContext context)
    {
        await mediator.Send(new RegisterDoctorCommand(request.FirstName, request.LastName, request.SpecialtyId));

        return new RegisterDoctorResponse();
    }
}