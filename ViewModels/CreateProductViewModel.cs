using Microsoft.AspNetCore.Http;
namespace MyFirstWebsite.ViewModels;

public class CreateProductViewModel
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public IFormFile? FileUpload { get; set; }
}