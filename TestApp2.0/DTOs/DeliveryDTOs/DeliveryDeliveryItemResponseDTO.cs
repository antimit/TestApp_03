namespace TestApp2._0.DTOs.DeliveryDTOs;

public class DeliveryDeliveryItemResponseDTO
{
    public int ProductId { get; set; }

    public decimal SalesUnitPrice { get; set; }

    public decimal TotalCost { get; set; }
    public int ItemWeight { get; set; }
    public int ItemVolume { get; set; }
}