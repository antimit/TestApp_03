using Microsoft.AspNetCore.Mvc;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.CustomerDTOs;
using TestApp2._0.DTOs.VehicleDTOs;
using TestApp2._0.Services;

namespace TestApp2._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly VehicleService _vehicleService;

    public VehiclesController(VehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet("GetAllVehicles")]
    public async Task<ActionResult<ApiResponse<List<VehicleResponseDTO>>>> GetAllVehicles()
    {
        var response = await _vehicleService.GetAllVehiclesAsync();

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpPost("RegisterVehicle")]
    public async Task<ActionResult<ApiResponse<VehicleResponseDTO>>> RegisterVehicle(
        [FromBody] VehicleAddDTO vehicleDto)
    {
        var response = await _vehicleService.CreateVehicleAsync(vehicleDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpPut("UpdateVehicleStatus")]
    public async Task<ActionResult<ConfirmationResponseDTO>> UpdateVehicleStatus(
        [FromBody] VehicleStatusUpdateDTO statusDto)
    {
        var response = await _vehicleService.UpdateVehicleStatusAsync(statusDto);

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }
}