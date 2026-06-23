using Microsoft.AspNetCore.Mvc;
namespace ProductApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[]
        {
            new { Id = 1, Name = "Laptop", Price = 55000 },
            new { Id = 2, Name = "Mouse", Price = 700 }
        });
    }
}