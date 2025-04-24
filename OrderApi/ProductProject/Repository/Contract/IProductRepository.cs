using OrderApi.ProductProject.Entities;
using OrderApi.ProductProject.Models;

namespace OrderApi.ProductProject.Repository.Contract;
public interface IProductRepository
{
    Task Add(AddProductModel model);

    public Task<Product> CheckById(int id);
}
