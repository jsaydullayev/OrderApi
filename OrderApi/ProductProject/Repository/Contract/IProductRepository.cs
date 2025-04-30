using OrderApi.ProductProject.Entities;
using OrderApi.ProductProject.Models;

namespace OrderApi.ProductProject.Repository.Contract;
public interface IProductRepository
{
    Task<int> Add(AddProductModel model);

    Task<Product> GetById(int id);

    Task SaveChanges();
}
