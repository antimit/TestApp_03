using Microsoft.AspNetCore.Mvc;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.CustomerDTOs;
using TestApp2._0.Services;

namespace TestApp2._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("RegisterCustomer")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> RegisterCustomer(
        [FromBody] CustomerAddDTO customerDto)
    {
        var response = await _customerService.AddCustomerAsync(customerDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpGet("GetAllCustomers")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> GetAllCustomers()
    {
        var response = await _customerService.GetAllCustomersAsync();
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpGet("GetCustomerById/{id}")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> GetCustomerById(int id)
    {
        var response = await _customerService.GetCustomerByIdAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpPut("UpdateCustomer")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateCustomer(
        [FromBody] CustomerUpdateDTO customerDto)
    {
        var response = await _customerService.UpdateCustomerAsync(customerDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }

    [HttpDelete("DeleteCustomer/{id}")]
    public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCustomer(int id)
    {
        var response = await _customerService.DeleteCustomerAsync(id);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }
}