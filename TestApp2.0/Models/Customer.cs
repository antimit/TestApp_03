using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TestApp2._0.Models;


[Index(nameof(Email), Name = "IX_Email_Unique", IsUnique = true)]
public class Customer
{
    public int  CustomerId { get; set; }
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Last Name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "PhoneNumber is required.")]
    public string PhoneNumber { get; set; }
    
    public Stop? Stop { get; set; }
    
}