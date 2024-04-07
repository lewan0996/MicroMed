using Doctors.Services;
using Grpc.Core;
using MediatR;

namespace Doctors.API.Services;

public class DoctorsService : API.DoctorsService.DoctorsServiceBase
{
    private readonly IMediator _mediator;

    public DoctorsService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<RegisterDoctorResponse> RegisterDoctor(RegisterDoctorRequest request, ServerCallContext context)
    {
        await _mediator.Send(new RegisterDoctorCommand(request.FirstName, request.LastName, request.SpecialtyId));

        return new RegisterDoctorResponse();
    }
}