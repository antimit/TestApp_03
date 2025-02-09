using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.ProductDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<ApiResponse<ProductResponseDTO>> CreateProductAsync(ProductAddDTO productDto)
        {
            try
            {
                // Check if product name already exists (case-insensitive)
                if (await _context.Products.AnyAsync(p => p.Name.ToLower() == productDto.Name.ToLower()))
                {
                    return new ApiResponse<ProductResponseDTO>(400, "Product name already exists.");
                }
                
                // Manual mapping from DTO to Model
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    SalesUnitPrice = productDto.SalesUnitPrice,
                    Volume = productDto.Volume,
                    Weight = productDto.Weight
                };
                // Add product to the database
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                // Map to ProductResponseDTO
                
                var productResponse = new ProductResponseDTO
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    SalesUnitPrice = product.SalesUnitPrice,
                    Description = product.Description,
                    Weight = product.Weight,
                    Volume = product.Volume
                };
                return new ApiResponse<ProductResponseDTO>(200, productResponse);
            }
            catch (Exception ex)
            {
                // Log the exception (implementation depends on your logging setup)
                return new ApiResponse<ProductResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    
    public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsAsync()
    {
        try
        {
            var products = await _context.Products
                .AsNoTracking()
                .ToListAsync();
            var productList = products.Select(p => new ProductResponseDTO
            {
                ProductId = p.ProductId,
                Name = p.Name,
                SalesUnitPrice = p.SalesUnitPrice,
                Description = p.Description,
                Weight = p.Weight,
                Volume = p.Volume
            }).ToList();
            return new ApiResponse<List<ProductResponseDTO>>(200, productList);
        }
        catch (Exception ex)
        {
            // Log the exception
            return new ApiResponse<List<ProductResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
    
    
}