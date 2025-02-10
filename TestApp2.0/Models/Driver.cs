using System.ComponentModel.DataAnnotations;

namespace TestApp2._0.Models;

public class Driver
{
    [Key] public int DriverId { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
    public string FirstName { get; set; }


    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }


    [Required(ErrorMessage = "PhoneNumber is required.")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Driver status is required.")]
    public DriverStatus Status { get; set; }

    public Transportation? Transportation { get; set; }
}