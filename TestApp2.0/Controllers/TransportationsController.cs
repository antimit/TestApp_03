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
    
    [HttpGet("GetAllTransportations")]
    public async Task<ActionResult<ApiResponse<List<TransportationResponseDTO>>>> GetAllTransportations()
    {
        var response = await _transportationService.GetAllTransportationsAsync();

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
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

    [HttpPut("UpdateTransportationStatus")]
    public async Task<ActionResult<ConfirmationResponseDTO>> UpdateTransportationStatus(
        [FromBody] TransportationStatusUpdateDTO statusDto)
    {
        var response = await _transportationService.UpdateTransportationStatusAsync(statusDto);

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpDelete("DeleteTransportation/{id}")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCustomer(int id)
    {
        var response = await _transportationService.DeleteTransportationAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }
}