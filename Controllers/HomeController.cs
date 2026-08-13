using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebsite.Models;

namespace MyFirstWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ProductDB _db;

    public HomeController(ProductDB db)
    {
        _db = db;
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
        var product = await _db.Products.FirstOrDefaultAsync(product => product.Id == id);

        return View(product);
    }

    [HttpGet]
    public IActionResult Product()
    {
        var product = new Product
        {
            Name = "Sample Product",
            Price = 19.99m
        };
        return View(product);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
