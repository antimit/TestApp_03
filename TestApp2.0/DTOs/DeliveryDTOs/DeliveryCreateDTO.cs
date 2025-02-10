using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.DeliveryDTOs;

public class DeliveryCreateDTO
{
    public int? StopId { get; set; }


    public List<DeliveryDeliveryItemCreateDTO> DeliveryItems { get; set; }

    [Required] public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;
}