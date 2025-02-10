using Microsoft.AspNetCore.Mvc;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.StopDTOs;
using TestApp2._0.Services;

namespace TestApp2._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StopsController : ControllerBase
{
    private readonly StopService _stopService;

    public StopsController(StopService stopService)
    {
        _stopService = stopService;
    }


    [HttpPost("RegisterStop")]
    public async Task<ActionResult<ApiResponse<StopResponseDTO>>> RegisterStops(
        [FromBody] StopCreateDTO stopDto)
    {
        var response = await _stopService.CreateStopAsync(stopDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpPut("UpdateStopStatus")]
    public async Task<ActionResult<ConfirmationResponseDTO>> UpdateVehicleStatus(
        [FromBody] StopStatusUpdateDTO statusDto)
    {
        var response = await _stopService.UpdateStopStatusAsync(statusDto);

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpGet("GetAllStops")]
    public async Task<ActionResult<ApiResponse<List<StopResponseDTO>>>> GetAllVehicles()
    {
        var response = await _stopService.GetAllStopsAsync();

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }
}