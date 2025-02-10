using Microsoft.EntityFrameworkCore;
using TestApp2._0.Data;
using TestApp2._0.Models;
using TestApp2._0.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
                builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
                builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("TestApp")));
        
                builder.Services.AddScoped<CustomerService>();
        
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
                options.JsonSerializerOptions.MaxDepth = 64;             });


        
        
        
        var app = builder.Build();
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