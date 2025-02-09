using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.Models;
using TestApp2._0.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // This will use the property names as defined in the C# model
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // Configure EF Core with SQL Server
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("TestApp")));
        
        // Registering the CustomerService
        builder.Services.AddScoped<CustomerService>();
        
        // Registering the CustomerService
        builder.Services.AddScoped<VehicleService>();
        
        builder.Services.AddScoped<DriverService>();
        
        builder.Services.AddScoped<ProductService>();
        
        builder.Services.AddScoped<DeliveryItemService>();

        builder.Services.AddScoped<DeliveryService>();
        
        builder.Services.AddScoped<AddressService>();

        builder.Services.AddScoped<StopService>();

        builder.Services.AddScoped<TransportationService>();
        
        
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                options.JsonSerializerOptions.MaxDepth = 64; // Increase depth limit
            });


        
        
        
        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        
       // await app.MigrateDbAsync();

        app.Run();
    }
}