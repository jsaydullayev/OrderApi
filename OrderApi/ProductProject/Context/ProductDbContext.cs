using Microsoft.EntityFrameworkCore;
using OrderApi.ProductProject.Entities;

namespace OrderApi.ProductProject.Context;
public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.CreatedTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
