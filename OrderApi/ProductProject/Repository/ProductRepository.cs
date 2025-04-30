using Microsoft.EntityFrameworkCore;
using OrderApi.ProductProject.Context;
using OrderApi.ProductProject.Entities;
using OrderApi.ProductProject.Models;
using OrderApi.ProductProject.Repository.Contract;

namespace OrderApi.ProductProject.Repository;
public class ProductRepository(ProductDbContext productDb) : IProductRepository
{

    private readonly ProductDbContext _productDb = productDb;
    
    public async Task<int> Add(AddProductModel model)
    {
        var product = new Product
        {
            Id = _productDb.Products.Count() + 1,
            ProductName = model.ProductName
        };
        await _productDb.Products.AddAsync(product);
        await _productDb.SaveChangesAsync();
        return product.Id;
    }

    public async Task<Product?> GetById(int id)
    {
        var product = await _productDb.Products.FirstOrDefaultAsync(x => x.Id == id);
        return product;
    }

    public async Task SaveChanges() => await _productDb.SaveChangesAsync();
}
