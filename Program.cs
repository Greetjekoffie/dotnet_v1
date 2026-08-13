using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using MyFirstWebsite.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ProductDB>(options => options.UseInMemoryDatabase("products"));

builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "Product API",
         Description = "Making the Products you love",
         Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI(c =>
   {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
   });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapGet("/products", async (ProductDB db) => await db.Products.ToListAsync());
app.MapGet("/product/{id}", async (ProductDB db, int id) => await db.Products.FindAsync(id));
app.MapPost("/product", async (ProductDB db, Product product) =>
{
    await db.Products.AddAsync(product);
    await db.SaveChangesAsync();
    return Results.Created($"/product/{product.Id}", product);
});
app.MapPut("/product/{id}", async (ProductDB db, Product updateproduct, int id) =>
{
      var product = await db.Products.FindAsync(id);
      if (product is null) return Results.NotFound();
      product.Name = updateproduct.Name;
      product.Price = updateproduct.Price;
      await db.SaveChangesAsync();
      return Results.NoContent();
});
app.MapDelete("/product/{id}", async (ProductDB db, int id) =>
{
   var product = await db.Products.FindAsync(id);
   if (product is null)
   {
      return Results.NotFound();
   }
   db.Products.Remove(product);
   await db.SaveChangesAsync();
   return Results.Ok();
});

app.Run();
