﻿using TestApp2._0.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestApp2._0.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<DeliveryItem>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.TotalCost = entry.Entity.SalesUnitPrice * entry.Entity.OrderedCount;
            }
        }
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<DeliveryItem>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.TotalCost = entry.Entity.SalesUnitPrice * entry.Entity.OrderedCount;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
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
        

        modelBuilder.Entity<Customer>()
            .Property(s => s.LastName)
            .IsRequired(false);


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
            .WithOne(d => d.Stop)
            .HasForeignKey<Stop>(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Delivery>()
            .HasOne(s => s.Stop)
            .WithMany(d => d.Deliveries)
            .HasForeignKey(s => s.StopId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Delivery>()
            .HasMany(d => d.DeliveryItems)
            .WithOne(di => di.CurrentDelivery)
            .HasForeignKey(di => di.CurrentDeliveryId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<DeliveryItem>()
            .HasOne(d => d.Product)
            .WithMany()
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Transportation>()
            .HasOne(d => d.Driver)
            .WithOne(s => s.Transportation)
            .HasForeignKey<Transportation>(d => d.DriverId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Transportation>()
            .HasOne(d => d.Truck)
            .WithOne(s => s.Transportation)
            .HasForeignKey<Transportation>(d => d.DriverId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Transportation>()
            .HasMany(d => d.Stops)
            .WithOne(s => s.CurrentTransportation)
            .HasForeignKey(s => s.TransportationId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<DeliveryItem>()
            .HasKey(s => s.DeliveryItemId);
        modelBuilder.Entity<DeliveryItem>()
            .Property(s => s.DeliveryItemId)
            .ValueGeneratedOnAdd();


        base.OnModelCreating(modelBuilder);
    }


    public DbSet<Transportation> Transportations { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Vehicle> Vehicles { get; set; }

    public DbSet<Driver> Drivers { get; set; }

    public DbSet<DeliveryItem> DeliveryItems { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Delivery> Deliveries { get; set; }

    public DbSet<Stop> Stops { get; set; }

    public DbSet<Address> Addresses { get; set; }
}