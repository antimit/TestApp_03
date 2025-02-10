namespace TestApp2._0.DTOs.ProductDTOs;

public class ProductResponseDTO
{
    public int ProductId { get; set; }


    public string Name { get; set; }

    public decimal SalesUnitPrice { get; set; }

    public string? Description { get; set; }

    public int Weight { get; set; }

    public int Volume { get; set; }
}