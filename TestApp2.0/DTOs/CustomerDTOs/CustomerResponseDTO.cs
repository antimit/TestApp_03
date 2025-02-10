using TestApp2._0.Models;

namespace TestApp2._0.DTOs.CustomerDTOs;

public class CustomerResponseDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public string stopOrder { get; set; }
}