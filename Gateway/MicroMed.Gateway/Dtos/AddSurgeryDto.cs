using Clinics.API;

namespace MicroMed.Gateway.Dtos;

public class AddSurgeryDto
{
    public string Number { get; set; }
    public string Floor { get; set; }
    public List<int> EquipmentIds { get; set; }

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