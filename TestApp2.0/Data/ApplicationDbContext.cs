using TestApp2._0.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestApp2._0.Data;

public class ApplicationDbContext : DbContext
{
    // Constructor accepting DbContextOptions
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Transportation>()
            .HasOne(s => s.Driver)
            .WithOne(d => d.Transportation)
            .HasForeignKey<Transportation>(s => s.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Stop>()
            .HasOne(s => s.Customer)
            .WithOne(d => d.Stop)
            .HasForeignKey<Stop>(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Stop>()
            .Property(s => s.TransportationId)
            .IsRequired(false);

        modelBuilder.Entity<Transportation>()
            .Property(s => s.DepartureTime)
            .IsRequired(false);

        modelBuilder.Entity<Transportation>()
            .Property(s => s.UpdatedAt)
            .IsRequired(false);
        modelBuilder.Entity<Transportation>()
            .Property(s => s.ActualArrivalTime)
            .IsRequired(false);




        modelBuilder.Entity<Customer>()
            .Property(s => s.LastName)
            .IsRequired(false);

        // modelBuilder.Entity<Customer>()
        //     .Property(s => s.Stop)
        //     .IsRequired(false);

        modelBuilder.Entity<Customer>()
            .Property(s => s.PhoneNumber)
            .IsRequired(false);
            

        modelBuilder.Entity<Customer>()
            .Property(s => s.Email)
            .IsRequired(false);

        modelBuilder.Entity<DeliveryItem>()
            .HasIndex(d => d.SalesUnitPrice)
            .HasDatabaseName("IX_DeliveryItem_SalesUnitPrice")
            .IsUnique(false);
        
        modelBuilder.Entity<DeliveryItem>()
            .HasIndex(d => d.ItemWeight)
            .HasDatabaseName("IX_DeliveryItem_ItemWeight")
            .IsUnique(false);

        modelBuilder.Entity<Stop>()
            .HasIndex(s => s.CustomerId)
            .HasDatabaseName("IX_Stops_CustomerId")
            .IsUnique(false);


        modelBuilder.Entity<Stop>()
            .HasOne(s => s.Customer)
            .WithOne(d =>d.Stop)
            .HasForeignKey<Stop>(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
       
        
        modelBuilder.Entity<Delivery>()
            .HasOne(s => s.Stop)
            .WithMany(d =>d.Deliveries)
            .HasForeignKey(s => s.StopId)
            .OnDelete(DeleteBehavior.Cascade);
            
        
        


        base.OnModelCreating(modelBuilder);

    }





    public DbSet<Transportation> Transportations { get; set; }
    
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Vehicle> Vehicles { get; set; }
    
    public DbSet<Driver> Drivers{ get; set; }
    
    public DbSet<DeliveryItem> DeliveryItems{ get; set; }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Delivery> Deliveries { get; set; }
    
    public DbSet<Stop> Stops { get; set; }
    
    public DbSet<Address> Addresses { get; set; }
    
}