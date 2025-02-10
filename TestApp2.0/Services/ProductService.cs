using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.DTOs;
using TestApp2._0.DTOs.ProductDTOs;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<ApiResponse<ProductResponseDTO>> CreateProductAsync(ProductAddDTO productDto)
    {
        try
        {
            if (await _context.Products.AnyAsync(p => p.Name.ToLower() == productDto.Name.ToLower()))
            {
                return new ApiResponse<ProductResponseDTO>(400, "Product name already exists.");
            }

            var product = _mapper.Map<Product>(productDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productResponse = _mapper.Map<ProductResponseDTO>(product);
            return new ApiResponse<ProductResponseDTO>(200, productResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsAsync()
    {
        try
        {
            var products = await _context.Products
                .AsNoTracking()
                .ToListAsync();
            var productList = _mapper.Map<List<ProductResponseDTO>>(products);
            return new ApiResponse<List<ProductResponseDTO>>(200, productList);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<ProductResponseDTO>>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }
    
    
    
    public async Task<ApiResponse<ProductResponseDTO>> UpdateProductAsync(ProductUpdateDTO productDto)
    {
        try
        {
          
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productDto.ProductId);
        
            if (existingProduct == null)
            {
                return new ApiResponse<ProductResponseDTO>(404, "Product not found.");
            }

           
            existingProduct.SalesUnitPrice = productDto.SalesUnitPrice;

           
            await _context.SaveChangesAsync();
            
            var updatedProductResponse = _mapper.Map<ProductResponseDTO>(existingProduct);

            return new ApiResponse<ProductResponseDTO>(200, updatedProductResponse);
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductResponseDTO>(500,
                $"An unexpected error occurred while processing your request, Error: {ex.Message}");
        }
    }

}