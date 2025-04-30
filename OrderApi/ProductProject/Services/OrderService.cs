using Hangfire;
using OrderApi.ProductProject.Entities;
using OrderApi.ProductProject.Repository.Contract;
using OrderApi.ProductProject.Services.Contract;
using System.ComponentModel.Design.Serialization;

namespace OrderApi.ProductProject.Services;
public class OrderService
    (
        IBackgroundJobClient backgroundJobClient,
        ILogger<ProductService> logger,
        IProductRepository productRepository
    ) : IOrderService
{
    public async Task<string> CompleteTaskAsync(int id)
    {
        var order = await productRepository.GetById(id);
        if(order == null || order.IsFinishedOrder)
        {
            if(order is not null && !string.IsNullOrEmpty(order.HangfireJobId))
                backgroundJobClient.Delete(order.HangfireJobId);

            return "";
        }
        if (!string.IsNullOrEmpty(order.HangfireJobId))
        {
            backgroundJobClient.Delete(order.HangfireJobId);
            return $"Buyurtma uchun {order} vaqt o‘chirildi: {order.HangfireJobId}";
        }

        order.IsFinishedOrder = true;
        order.CreatedTime = DateTime.UtcNow;
        order.HangfireJobId = null;

        await productRepository.SaveChanges();

        return $"order {order} muvofaqqiyatli yakunlandi";
    }


    [AutomaticRetry(Attempts = 0)]
    public async Task<Tuple<Product?,string?>> MarkOrderAsFinishedAsync(int id)
    {
        var order = await productRepository.GetById(id);

        if(order == null || order.IsFinishedOrder)
        {
            if(order is not null && !string.IsNullOrEmpty(order.HangfireJobId))
                backgroundJobClient.Delete(order.HangfireJobId);

            return new (null,"");
        }

        order.IsFinishedOrder = true;
        order.CreatedTime = DateTime.UtcNow;

        await productRepository.SaveChanges();

        return new(order,$"Buyurtma avtomatik yakunlanadi");
    }

    public async Task<string> StartOrderAsync(int id)
    {
        var order = await productRepository.GetById(id);

        if (order is null || order.IsFinishedOrder)
        {
            return "";
        }

        var jobId = backgroundJobClient.Schedule(
            () => MarkOrderAsFinishedAsync(id),
            TimeSpan.FromMinutes(3));

        order.HangfireJobId = jobId;
        order.CreatedTime = DateTime.UtcNow;

        await productRepository.SaveChanges();

        return ($"Id : {id} 3 minutdan so'ng tayyor bo'ladi. Job ID : {jobId}");
    }
}
