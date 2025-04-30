using OrderApi.ProductProject.Entities;

namespace OrderApi.ProductProject.Services.Contract;
public interface IOrderService
{
    Task<string> StartOrderAsync(int id);
    Task<string> CompleteTaskAsync(int id);
    Task<Tuple<Product,string>> MarkOrderAsFinishedAsync(int id);
}
