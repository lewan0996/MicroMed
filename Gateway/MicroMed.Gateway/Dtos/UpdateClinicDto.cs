using Clinics.API;

namespace MicroMed.Gateway.Dtos;

internal class UpdateClinicDto
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string AdditionalInfo { get; set; }

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