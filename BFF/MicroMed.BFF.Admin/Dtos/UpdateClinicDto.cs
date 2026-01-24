using Clinics.Contracts;

namespace MicroMed.BFF.Admin.Dtos;

internal record UpdateClinicDto(string Name, string City, string Street, string StreetNumber, string AdditionalInfo)
{
    public UpdateClinicRequest ToRpcRequest(int clinicId)
        => new()
        {
            Name = Name,
            AdditionalInfo = AdditionalInfo,
            City = City,
            Street = Street,
            StreetNumber = StreetNumber,
            ClinicId = clinicId
        };
}