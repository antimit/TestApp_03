using Microsoft.EntityFrameworkCore;
using TestApp2._0.Services;

namespace TestApp2._0.Data;

public static class DataExtensions
{
    public static  async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // await dbContext.Database.MigrateAsync();
        await dbContext.Database.EnsureDeletedAsync(); // Drops the database if it exists
        await dbContext.Database.EnsureCreatedAsync();
        var dataSeeder = new DataSample(dbContext);
        dataSeeder.SeedDatabase();
        
    }
}