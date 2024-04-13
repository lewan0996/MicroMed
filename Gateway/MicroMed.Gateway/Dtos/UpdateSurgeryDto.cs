using Clinics.API;

namespace MicroMed.Gateway.Dtos;

public class UpdateSurgeryDto
{
    public string Number { get; set; }
    public string Floor { get; set; }
    public List<int> EquipmentIds { get; set; }

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