using OrderApi.ProductProject.Models;
using OrderApi.ProductProject.Repository.Contract;
using OrderApi.ProductProject.Services.Contract;

namespace OrderApi.ProductProject.Services;
public class ProductService(IProductRepository productRepository, IOrderService orderService)
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IOrderService _orderService = orderService;

    public async Task<string> AddOrder(AddProductModel model)
    {
        var product = await _productRepository.Add(model);
        var hangfire = await _orderService.StartOrderAsync(product);
        return hangfire;
    }

    public async Task<object> CheckOrder(int id)
    {
        var product = await _productRepository.GetById(id);
        if (product is null)
        {
            return "Bunday order mavjud emas";
        }
        var hangfire = await _orderService.MarkOrderAsFinishedAsync(id);
        return hangfire;
    }
}
