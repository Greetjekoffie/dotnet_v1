using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebsite.Models;

namespace MyFirstWebsite.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _products;

    public HomeController(IProductRepository products)
    {
        _products = products;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewBag.Name = "Roman"; 
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpGet("Home/Product/{id}")]
    public async Task<IActionResult> Product(int id){
        var product = await _products.GetByIdAsync(id);

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Product()
    {
        var product = await _db.Products.LastOrDefaultAsync();
        if (product == null){
            product = new Product
            {
                Name = "Sample Product",
                Price = 19.99m
            };
        }
        return View(product);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
