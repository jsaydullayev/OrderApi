using OrderApi.ProductProject.Context;
using OrderApi.ProductProject.Entities;
using OrderApi.ProductProject.Models;
using OrderApi.ProductProject.Repository.Contract;

namespace OrderApi.ProductProject.Repository;
public class ProductRepository(ProductDbContext productDb) : IProductRepository
{

    private readonly ProductDbContext _productDb = productDb;
    
    public async Task Add(AddProductModel model)
    {
        var product = new Product
        {
            Id = _productDb.Products.Count() + 1,
            ProductName = model.ProductName,
            CreatedTime = TimeOnly.FromDateTime(DateTime.Now)
        };
        await _productDb.Products.AddAsync(product);
        await _productDb.SaveChangesAsync();
    }

    public Task<Product?> CheckById(int id)
    {
        var product = _productDb.Products.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(product);
    }
}
