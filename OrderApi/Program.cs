using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using OrderApi.ProductProject.Context;
using OrderApi.ProductProject.Repository;
using OrderApi.ProductProject.Repository.Contract;
using OrderApi.ProductProject.Services;
using OrderApi.ProductProject.Services.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProductDbContext")));

var connectionString = builder.Configuration.GetConnectionString("ProductDbContext");
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions
    {
        SchemaName = "hangfire" // Optional schema separation
    }));

builder.Services.AddHangfireServer(options => {
    options.WorkerCount = Environment.ProcessorCount * 5;
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IOrderService,OrderService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
