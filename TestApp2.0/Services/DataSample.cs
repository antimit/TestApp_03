using TestApp2._0.Data;
using TestApp2._0.Models;

namespace TestApp2._0.Services;

public class DataSample
{
    private static Random Random = new Random();
    private readonly ApplicationDbContext _context;


    private Dictionary<string, decimal> materials = new Dictionary<string, decimal>()
    {
        { "Beer Crate 1", 146.90m },
        { "Beer Crate 2", 286.30m },
        { "Birel Pomelo", 119.90m },
        { "PU Keg 50l", 2468.50m },
        { "GA KEG 30l", 1120m }
    };

    string[] firstNames = { "John", "Jane", "Michael", "Sarah", "Chris", "Emma", "Daniel", "Olivia" };
    string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis" };


    public DataSample(ApplicationDbContext context)
    {
        _context = context;
    }

    public void SeedDatabase()
    {
        _context.Transportations.RemoveRange(_context.Transportations);
        _context.SaveChanges();

        var shipment = CreateRandomShipment();
        _context.Transportations.Add(shipment);
        _context.SaveChanges();


        var stops = CreateStops(shipment);
        _context.Stops.AddRange(stops);
        _context.SaveChanges();
    }

    private Transportation CreateRandomShipment()
    {
        var transportation = new Transportation()
        {
            Truck = new Vehicle()
            {
                LicensePlate = "PLZEN001",
                Make = "Scania", Model = "R450", VIN = "5N3AA08D68N901917", Status = VehicleStatus.Active,
                IsAvailable = true
            },
            Driver = new Driver
            {
                FirstName =
                    firstNames[Random.Next(firstNames.Length)] + " " + Guid.NewGuid().ToString().Substring(0, 4),
                LastName = lastNames[Random.Next(lastNames.Length)],
                Email = "user" + Guid.NewGuid().ToString().Substring(0, 8) + "@example.com",
                PhoneNumber = "+420" + Random.Next(100000000, 999999999).ToString(),
                Transportation = null,
                Status = DriverStatus.Active
            },
            TransportationStatus = TransportationStatus.Pending
        };
        return transportation;
    }


    private List<Stop> CreateStops(Transportation transportation)
    {
        List<Stop> stops = new();
        int count = Random.Next(3, 30);
        int order = 1;

        stops.Add(new Stop
        {
            StopOrder = Guid.NewGuid().ToString(),
            DistanceFromPreviousStop = Random.Next(100, 500),
            Customer = CreateDepot(),
            Address = CreateRandomAddress(),
            Deliveries = new List<Delivery>(),
            CurrentTransportation = transportation,
            TransportationId = transportation.TransportationId,
            Status = StopStatus.Pending
        });

        for (; order < count; order++)
        {
            var customer = CreateCustomer();

            var stop = new Stop
            {
                StopOrder = Guid.NewGuid().ToString(),
                DistanceFromPreviousStop = (Random.NextDouble() + 10) * 100,
                CurrentTransportation = transportation,
                TransportationId = transportation.TransportationId,
                Customer = customer,
                CustomerId = customer.CustomerId,
                Address = CreateRandomAddress(),
                Deliveries = new List<Delivery>()
            };


            int deliveryCount = Random.Next(1, 4);
            for (int i = 0; i < deliveryCount; i++)
            {
                var delivery = CreateDelivery(stop);
                stop.Deliveries.Add(delivery);
            }

            stops.Add(stop);
        }

        stops.Add(new Stop
        {
            StopOrder = Guid.NewGuid().ToString(),
            Customer = CreateDepot(),
            Address = CreateRandomAddress(),
            CurrentTransportation = transportation,
            TransportationId = transportation.TransportationId
        });

        return stops;
    }


    private Address CreateRandomAddress()
    {
        string[] streets = { "Main St", "Broadway", "High St", "Market St", "Sunset Blvd", "Elm St", "Oak St" };
        string[] cities = { "Prague", "Brno", "Ostrava", "Plzen", "Liberec", "Olomouc" };
        string[] countries = { "Czech Republic" };

        return new Address
        {
            Street = streets[Random.Next(streets.Length)] + " " + Random.Next(1, 999),
            City = cities[Random.Next(cities.Length)],
            PostalCode = Random.Next(10000, 99999).ToString(),
            Country = countries[0]
        };
    }


    private Customer CreateDepot()
    {
        return new Customer
        {
            FirstName = "DC Plzeň",
            LastName = null,
            Email = null,
            PhoneNumber = null,
            Stop = null
        };
    }

    private Customer CreateCustomer()
    {
        return new Customer()
        {
            FirstName = firstNames[Random.Next(firstNames.Length)] + " " + Guid.NewGuid().ToString().Substring(0, 4),
            LastName = lastNames[Random.Next(lastNames.Length)],
            Email = "user" + Guid.NewGuid().ToString().Substring(0, 8) + "@example.com",
            PhoneNumber = "+420" + Random.Next(1000000000, 1999999999).ToString(),
        };
    }


    private Delivery CreateDelivery(Stop stop)
    {
        Delivery delivery = new Delivery()
        {
            Stop = stop,
            StopId = stop.StopId,
            DeliveryItems = new List<DeliveryItem>()
        };

        for (int i = 0; i < Random.Next(1, 1000); i++)
        {
            delivery.DeliveryItems.Add(CreateDeliveryItem());
        }

        return delivery;
    }


    private DeliveryItem CreateDeliveryItem()
    {
        Product product = CreateProduct();

        decimal salesUnitPrice = Random.Next(10, 10000);
        int orderedCount = Random.Next(10, 1000);

        return new DeliveryItem
        {
            ProductId = product.ProductId,
            Name = Guid.NewGuid().ToString(),
            SalesUnitPrice = Random.Next(10, 10000),
            OrderedCount = orderedCount,
            DeliveredCount = null,
            CurrentDeliveryId = null,
            CurrentDelivery = null,
            TotalCost = salesUnitPrice * orderedCount,
            ItemWeight = Random.Next(10, 100),
            ItemVolume = Random.Next(10, 100)
        };
    }


    private Product CreateProduct()
    {
        var product = new Product
        {
            Name = $"Product-{Guid.NewGuid().ToString()[..6]}",
            SalesUnitPrice = Random.Next(100, 150000),
            Description = $"Description-{Guid.NewGuid().ToString()[..10]}",
            Weight = Random.Next(1, 1000),
            Volume = Random.Next(1, 150)
        };

        _context.Products.Add(product);
        _context.SaveChanges();
        return product;
    }
}