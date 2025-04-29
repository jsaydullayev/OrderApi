using Microsoft.AspNetCore.Mvc;
using OrderApi.ProductProject.Models;
using OrderApi.ProductProject.Services;

namespace OrderApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController(ProductService productService) : ControllerBase
{
    private readonly ProductService _productService = productService;

    [HttpPost]  
    public async Task<IActionResult> AddOrder(AddProductModel model)
    {
        var product = await _productService.AddOrder(model);
        return Ok(product);
        }

    [HttpGet]
    public async Task<IActionResult> CheckOrder(int id)
    {
        var product = await _productService.CheckOrder(id);
        return Ok(product);
    }
}
