using Microsoft.AspNetCore.Mvc;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.AddressDTOs;
using TestApp2._0.Services;

namespace TestApp2._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressesController : ControllerBase
{
    private readonly AddressService _addressService;

    public AddressesController(AddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpPost("RegisterAddress")]
    public async Task<ActionResult<ApiResponse<AddressResponseDTO>>> RegisterAddress(
        [FromBody] AddressAddDTO addressDto)
    {
        var response = await _addressService.AddAddressAsync(addressDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpGet("GetAddressById/{id}")]
    public async Task<ActionResult<ApiResponse<AddressResponseDTO>>> GetAddressById(int id)
    {
        var response = await _addressService.GetAddressByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpDelete("DeleteAddress/{id}")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCustomer(int id)
    {
        var response = await _addressService.DeleteAddressAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }
}