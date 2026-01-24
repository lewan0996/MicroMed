using Clinics.Contracts;

namespace MicroMed.BFF.Admin.Dtos;

internal record UpdateSurgeryDto(string Number, string Floor, List<int> EquipmentIds)
{
    public UpdateSurgeryRequest ToRpcRequest(int clinicId, int surgeryId)
    {
        var request = new UpdateSurgeryRequest
        {
            ClinicId = clinicId,
            SurgeryId = surgeryId,
            Floor = Floor,
            Number = Number
        };

        request.EquipmentIds.AddRange(EquipmentIds);

        return request;
    }
}