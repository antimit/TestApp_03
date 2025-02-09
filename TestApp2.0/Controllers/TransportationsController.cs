using Microsoft.AspNetCore.Mvc;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.TransportationDTOs;
using TestApp2._0.Services;

namespace TestApp2._0.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TransportationsController : ControllerBase
{
    private readonly TransportationService _transportationService;

    public TransportationsController(TransportationService transportationService)
    {
        _transportationService = transportationService;
    }
    
    [HttpPost("RegisterTransportation")]
    public async Task<ActionResult<ApiResponse<TransportationResponseDTO>>> RegisterTransportation(
        [FromBody] TransportationCreateDTO transportationDto)
    {
        var response = await _transportationService.CreateTransportationAsync(transportationDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }
    
    // [HttpGet("GetAllVehicles")]
    // public async Task<ActionResult<ApiResponse<List<TransportationResponseDTO>>>> GetAllVehicles()
    // {
    //     var response = await _transportationService.GetAllTranAsync();
    //
    //     if (response.StatusCode != 200)
    //     {
    //         return StatusCode(response.StatusCode, response);
    //     }
    //
    //     return Ok(response);
    // }
    
}