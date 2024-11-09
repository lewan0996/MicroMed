using Clinics.Contracts;

namespace MicroMed.BFF.Admin.Dtos;

internal record AddSurgeryDto(string Number, string Floor, List<int> EquipmentIds)
{
    public AddSurgeryRequest ToRpcRequest(int clinicId)
    {
        var request = new AddSurgeryRequest
        {
            ClinicId = clinicId,
            Floor = Floor,
            Number = Number
        };

        request.EquipmentIds.AddRange(EquipmentIds);

        return request;
    }
}