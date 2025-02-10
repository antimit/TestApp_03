using System.ComponentModel.DataAnnotations;

namespace TestApp2._0.DTOs.StopDTOs;

public class StopDeliveryCreate
{
    [Required(ErrorMessage = "Delivery ID is required.")]
    public int DeliveryId { get; set; }
}
