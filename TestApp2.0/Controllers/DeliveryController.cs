using Microsoft.AspNetCore.Mvc;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.DeliveryDTOs;
using TestApp2._0.Services;

namespace TestApp2._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryController : ControllerBase
{
    private readonly DeliveryService _deliveryService;

    public DeliveryController(DeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpGet("GetAllDeliveries")]
    public async Task<ActionResult<ApiResponse<DeliveryResponseDTO>>> GetAllCustomers()
    {
        var response = await _deliveryService.GetAllDeliveriesAsync();
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpPost("AddDeliveries")]
    public async Task<ActionResult<ApiResponse<DeliveryResponseDTO>>> RegisterDriver(
        [FromBody] DeliveryCreateDTO deliveryItemDto)
    {
        var response = await _deliveryService.CreateDeliveryAsync(deliveryItemDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpGet("GetDeliveryById/{id}")]
    public async Task<ActionResult<ApiResponse<DeliveryResponseDTO>>> GetDeliveryById(int id)
    {
        var response = await _deliveryService.GetDeliveryByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpPut("UpdateDeliveryStatus")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateDeliveryStatus(
        [FromBody] DeliveryStatusUpdateDTO statusDto)
    {
        var response = await _deliveryService.UpdateDeliveryStatusAsync(statusDto);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpDelete("DeleteDelivery")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteDelivery(int deliveryId)
    {
        var response = await _deliveryService.DeleteDeliveryAsync(deliveryId);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }
}