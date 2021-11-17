namespace ProductApi.Models;

record Product(Guid Id, string Name, string Manufacturer, string Type, string Price, int Quantity, string Dimensions, string Weight, string Description);