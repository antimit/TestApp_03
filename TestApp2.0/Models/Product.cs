using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp2._0.Models;

public class Product
{
    public int ProductId { get; set; }

   
    public string Name { get; set; }

    public decimal SalesUnitPrice { get; set; }

    public string? Description { get; set; }

    public int Weight { get; set; }

    public int Volume { get; set; }
}