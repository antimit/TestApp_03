using Microsoft.AspNetCore.Mvc;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.ProductDTOs;
using TestApp2._0.Services;
using Microsoft.AspNetCore.Mvc;

namespace TestApp2._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("GetAllProducts")]
    public async Task<ActionResult<ApiResponse<List<ProductResponseDTO>>>> GetAllProductsAsync()
    {
        var response = await _productService.GetAllProductsAsync();
        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }
    
    [HttpPut("UpdateProduct")]
    public async Task<ActionResult<ConfirmationResponseDTO>> UpdateProduct(
        [FromBody] ProductUpdateDTO statusDto)
    {
        var response = await _productService.UpdateProductAsync (statusDto);

        if (response.StatusCode != 200)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Ok(response);
    }


    [HttpPost("AddProduct")]
    public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> RegisterProduct(
        [FromBody] ProductAddDTO productDto)
    {
        var response = await _productService.CreateProductAsync(productDto);
        if (response.StatusCode != 200)
        {
            return StatusCode((int)response.StatusCode, response);
        }

        return Ok(response);
    }
}