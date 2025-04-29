namespace OrderApi.ProductProject.Services.Contract;
public interface IOrderService
{
    Task StartOrderAsync(int id);
    Task CompleteTaskAsync(int id);
    Task MarkOrderAsFinishedAsync(int id);
}
