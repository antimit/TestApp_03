using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.DeliveryDTOs;

public class DeliveryCreateDTO
{
    public List<DeliveryDeliveryItemCreateDTO> DeliveryItems { get; set; }

    
}