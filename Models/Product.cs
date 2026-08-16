using Microsoft.EntityFrameworkCore;
namespace MyFirstWebsite.Models;

public class Product
{
    public int Id { get; set; }
    public String Name { get; set; } = " ";
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
}

public class ProductDB : DbContext
{
    public ProductDB(DbContextOptions options) : base(options) { }
    public DbSet<Product> Products { get; set; } = null!;
}