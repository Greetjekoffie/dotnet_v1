using Minio;
using Minio.Exceptions;
using Minio.DataModel.Args;
using MyFirstWebsite.Models;
using MyFirstWebsite.ViewModels;

public class ProductService
{
    private readonly IProductRepository _products;
    private readonly IMinioClient _minioClient;

    public ProductService(IProductRepository products, IMinioClient minioClient)
    {
        _products = products;
        _minioClient = minioClient;
    }


    public async Task CreateAsync(CreateProductViewModel model)
    {
        var product = new Product
        {
            Name = model.Name,
            Price = model.Price
        };

        await _products.AddAsync(product);

        var productId = product.Id;

        if (model.FileUpload != null)
        {

            var bucketName = "products";
            var objectName = "images/" + productId + "/front_image";

            try
            {
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(model.FileUpload.OpenReadStream())
                    .WithObjectSize(model.FileUpload.Length)
                    .WithContentType(model.FileUpload.ContentType);
                await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                Console.WriteLine("Successfully uploaded " + objectName );
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
            }

        }
    }

    public async Task<MemoryStream> GetImageAsync(int productId)
    {
        var bucketName = "products";
        var objectName = "images/" + productId + "/front_image";

        var downloadStream = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithCallbackStream(x =>
            {
                x.CopyTo(downloadStream);
                downloadStream.Seek(0, SeekOrigin.Begin);
            });

        await _minioClient.GetObjectAsync(args);

        return downloadStream;
    }
}