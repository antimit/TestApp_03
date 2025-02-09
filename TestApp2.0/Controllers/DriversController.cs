using Microsoft.AspNetCore.Mvc;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.DriverDTOs;
using TestApp2._0.Services;

namespace TestApp2._0.Controllers;


[ApiController]
[Route("api/[controller]")]
public class DriversController: ControllerBase
{
    private readonly DriverService _driverService;

    
    public DriversController(DriverService driverService)
    {
        _driverService = driverService;
    }

    [HttpGet("GetAllDrivers")]
    public async Task<ActionResult<ApiResponse<List<DriverResponseDTO>>>> GetAllDrivers()
    {
        var response = await _driverService.GetAllDriversAsync();

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }
        return Ok(response);
    }
    
    [HttpGet("GetDriverById/{id}")]
    
    public async Task<ActionResult<ApiResponse<DriverResponseDTO>>> GetCustomerById(int id)
    {
        var response = await _driverService.GetDriverByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
        return Ok(response);
    }
    
    
    [HttpPost("RegisterDriver")]
    public async Task<ActionResult<ApiResponse<DriverResponseDTO>>> RegisterDriver(
        [FromBody] DriverAddDTO driverDto)
    {
        var response = await _driverService.AddDriverAsync(driverDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }
    
        return Ok(response);
    }

    [HttpPut("UpdateDriverStatus")]
    public async Task<ActionResult<ConfirmationResponseDTO>> UpdateDriverStatus(
        [FromBody] DriverUpdateStatusDTO statusDto)
    {
        var response = await _driverService.UpdateDriverStatusAsync(statusDto);

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }
}