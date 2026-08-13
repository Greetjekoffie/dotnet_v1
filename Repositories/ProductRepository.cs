using MyFirstWebsite.Models;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly ProductDB _db;

    public ProductRepository(ProductDB db)
    {
        _db = db;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _db.Products
            .FirstOrDefaultAsync(product => product.Id == id);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);

        if (product != null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }
    }
}