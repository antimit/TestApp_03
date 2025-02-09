using System.ComponentModel.DataAnnotations;
using TestApp2._0.Models;

namespace TestApp2._0.DTOs.StopDTOs;

public class StopStatusUpdateDTO
{
    [Required(ErrorMessage = "StopId is Required")]
    public int StopId { get; set; }
    
    [Required]
    [EnumDataType(typeof(StopStatus), ErrorMessage = "Invalid Stop Status.")]
    public StopStatus StopStatus { get; set; }
}