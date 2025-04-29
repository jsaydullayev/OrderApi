namespace OrderApi.ProductProject.Entities;
public class Product
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public bool IsFinishedOrder { get; set; }
    public string? HangfireJobId { get; set; }
    public DateTime CreatedTime { get; set; }

}
