using OrderApi.ProductProject.Entities;
using OrderApi.ProductProject.Models;
using OrderApi.ProductProject.Repository.Contract;

namespace OrderApi.ProductProject.Services;
public class ProductService(IProductRepository productRepository)
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<string> AddOrder(AddProductModel model)
    {
        var product = _productRepository.Add(model);
        return $"Orderingiz 15 minutdan keyin tayyor bo'ladi.\nOrderId: {product.Id}";
    }

    public async Task<object> CheckOrder(int id)
    {
        var product = await _productRepository.CheckById(id);
        if (product is null)
        {
            return "Bunday order mavjud emas";
        }
        if (TimeOnly.FromDateTime(DateTime.Now) - product.CreatedTime >= TimeSpan.FromMinutes(15))
        {
            return product;
        }
        else
        {
            var time = TimeOnly.FromDateTime(DateTime.Now) - product.CreatedTime;
            var remainingTime = TimeSpan.FromMinutes(15) - time;
            return($"Sizning productingiz hali tayyor emas. \nQolgan vaqt: {remainingTime.Minutes} minut");
        }
    }
}
