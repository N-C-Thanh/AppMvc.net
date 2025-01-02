using EX01.Models;

namespace EX01.Services;

public class ProductService : List<ProductModel>
{
    public ProductService()
    {
        this.AddRange([
            new() { Id = 1, Name = "Iphone X", Price = 1000},
            new() { Id = 2, Name = "Samsung ABC", Price = 500},
            new() { Id = 3, Name = "Sony XYZ", Price = 800},
            new() { Id = 4, Name = "Nokia BCD", Price = 100},
        ]);
    }
}