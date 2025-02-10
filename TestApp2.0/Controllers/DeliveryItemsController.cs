using Microsoft.AspNetCore.Mvc;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.DeliveryItemsDTOs;
using TestApp2._0.Models;
using TestApp2._0.Services;

namespace TestApp2._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryItemsController : ControllerBase
{
    private readonly DeliveryItemService _deliveryItemService;

    public DeliveryItemsController(DeliveryItemService deliveryItemService)
    {
        _deliveryItemService = deliveryItemService;
    }

    [HttpGet("GetAllDeliveryItems")]
    public async Task<ActionResult<ApiResponse<List<DeliveryItemResponseDTO>>>> GetAllDeliveryItems()
    {
        var response = await _deliveryItemService.GetAllDeliveryItemsAsync();

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpPost("AddDeliveryItem")]
    public async Task<ActionResult<ApiResponse<DeliveryItemResponseDTO>>> RegisterItemDelivery(
        [FromBody] DeliveryItemCreateDTO deliveryItemDto)
    {
        var response = await _deliveryItemService.CreateDeliveryItemAsync(deliveryItemDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpDelete("DeleteDeliveryItem")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteDeliveryItem(int deliveryItemId)
    {
        var response = await _deliveryItemService.DeleteDeliveryItemAsync(deliveryItemId);
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpGet("GetFilteredDeliveryItems")]
    public async Task<ActionResult<ApiResponse<List<DeliveryItemResponseDTO>>>> GetFilteredDeliveryItems(
        [FromQuery] decimal? maxSalesUnitPrice,
        [FromQuery] int? minItemWeight)
    {
        var response = await _deliveryItemService.GetFilteredDeliveryItemsAsync(maxSalesUnitPrice, minItemWeight);

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }
}