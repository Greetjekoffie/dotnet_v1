using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebsite.Models;
using MyFirstWebsite.ViewModels;
using Minio;
using Minio.DataModel.Args;
using System.Reflection;
namespace MyFirstWebsite.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _products;
    private readonly ProductService _productService;

    private readonly IMinioClient _minioClient;
    public HomeController(IProductRepository products, ProductService service, IMinioClient client)
    {
        _products = products;
        _productService = service;
        _minioClient = client;
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

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        await _productService.CreateAsync(model);

        return RedirectToAction(nameof(Index));
    }


    [HttpGet("Home/Product/{id}")]
    public async Task<IActionResult> Product(int id){
        var product = await _products.GetByIdAsync(id);

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Product()
    {
        var products = await _products.GetAllAsync();
        if (products == null || !products.Any()){
            return View( new Product
            {
                Id = -1,
                Name = "Sample Product",
                Price = 19.99m
            });
        }

        return View(products.LastOrDefault());
    }

    public async Task<IActionResult> GetImage(int productId)
    {
        var image = await _productService.GetImageAsync(productId);

        return File(image, "image/jpeg");
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUrl(string bucketID)
    {
        return Ok(await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(bucketID))
            .ConfigureAwait(false));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
