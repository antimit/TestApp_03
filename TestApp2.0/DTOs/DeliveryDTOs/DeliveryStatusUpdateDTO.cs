using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.DeliveryDTOs;

public class DeliveryStatusUpdateDTO
{
    [Required(ErrorMessage = "DeliveryId is Required")]
    public int DeliveryId { get; set; }
    
    [Required]
    [EnumDataType(typeof(DeliveryStatus), ErrorMessage = "Invalid Delivery Status.")]
    public DeliveryStatus DeliveryStatus { get; set; }
}