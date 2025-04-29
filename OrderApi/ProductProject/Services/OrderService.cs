using Hangfire;
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
    public async Task CompleteTaskAsync(int id)
    {
        var order = await productRepository.GetById(id);
        if(order == null || order.IsFinishedOrder)
        {
            if(order is not null && !string.IsNullOrEmpty(order.HangfireJobId))
                backgroundJobClient.Delete(order.HangfireJobId);

            return;
        }
        if (!string.IsNullOrEmpty(order.HangfireJobId))
        {
            backgroundJobClient.Delete(order.HangfireJobId);
            logger.LogInformation($"Deleted timeout job {order.HangfireJobId} for order {order}");
        }

        order.IsFinishedOrder = true;
        order.CreatedTime = DateTime.UtcNow;
        order.HangfireJobId = null;

        await productRepository.SaveChanges();

        logger.LogInformation($"order {order} completed by user");
    }


    [AutomaticRetry(Attempts = 0)]
    public async Task MarkOrderAsFinishedAsync(int id)
    {
        var order = await productRepository.GetById(id);

        if(order == null || order.IsFinishedOrder)
        {
            if(order is not null && !string.IsNullOrEmpty(order.HangfireJobId))
                backgroundJobClient.Delete(order.HangfireJobId);

            return;
        }

        order.IsFinishedOrder = true;
        order.CreatedTime = DateTime.UtcNow;

        await productRepository.SaveChanges();

        logger.LogInformation($"order {order} automatically marked as finished after timeout");
    }

    public async Task StartOrderAsync(int id)
    {
        var order = await productRepository.GetById(id);

        if (order is null || order.IsFinishedOrder)
        {
            return;
        }

        var jobId = backgroundJobClient.Schedule(
            () => MarkOrderAsFinishedAsync(id),
            TimeSpan.FromMinutes(3));

        order.HangfireJobId = jobId;
        order.CreatedTime = DateTime.UtcNow;

        await productRepository.SaveChanges();

        logger.LogInformation($"Id : {id} 3 minutdan so'ng tayyor bo'ladi. Job ID : {jobId}");
    }
}
