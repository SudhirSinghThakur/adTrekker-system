using AdImpressionService.Services;
using Amazon.S3;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAWSService<IAmazonS3>();
        builder.Services.AddSingleton<S3Uploader>();
        builder.Services.AddSingleton<DynamoLogger>();
        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();
        app.Run();
    }
}