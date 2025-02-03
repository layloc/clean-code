using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace AspNetSemester.Services;

public class MinioService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioService(string endpoint, string accessKey, string secretKey, string bucketName)
    {
        _minioClient = new MinioClient()
            .WithEndpoint("127.0.0.1", 9000)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(false)
            .Build(); // Теперь это возвращает IMinioClient

        _bucketName = bucketName;
        EnsureBucketExistsAsync().Wait();
    }
    private async Task EnsureBucketExistsAsync()
    {
        try
        {
            var args = new BucketExistsArgs().WithBucket(_bucketName);
            bool found = await _minioClient.BucketExistsAsync(args).ConfigureAwait(false);
        
            if (!found)
            {
                var makeArgs = new MakeBucketArgs()
                    .WithBucket(_bucketName)
                    .WithLocation("us-east-1"); // Добавьте регион

                await _minioClient.MakeBucketAsync(makeArgs).ConfigureAwait(false);
            }
        }
        catch (MinioException e)
        {
            Console.WriteLine($"Bucket error: {e.Message}");
            throw;
        }
    }

    public async Task<string> UploadFileAsync(string objectName, Stream stream)
    {
        try
        {
            // Перемотайте поток перед использованием
            if (stream.CanSeek) 
                stream.Seek(0, SeekOrigin.Begin);

            var args = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("application/octet-stream"); // Явно укажите Content-Type

            var result = await _minioClient.PutObjectAsync(args).ConfigureAwait(false);
            return $"{_bucketName}/{objectName}";
        }
        catch (MinioException e)
        {
            Console.WriteLine($"Upload error: {e.Message}");
            return null;
        }
    }

    public async Task<Stream> GetFileAsync(string objectName)
    {
        try
        {
            var memoryStream = new MemoryStream();
        
            var args = new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithCallbackStream(async stream => 
                {
                    await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                });

            await _minioClient.GetObjectAsync(args).ConfigureAwait(false);
            return memoryStream;
        }
        catch (MinioException e)
        {
            Console.WriteLine($"Download error: {e.Message}");
            return null;
        }
    }
}
