using TestApp2._0.Models;

namespace TestApp2._0.DTOs.StopDTOs;

public class StopDeliveryResponse
{
    public DeliveryStatus Status { get; set; }


    public decimal TotalValue { get; set; }


    public double TotalWeight { get; set; }


    public double TotalVolume { get; set; }
}