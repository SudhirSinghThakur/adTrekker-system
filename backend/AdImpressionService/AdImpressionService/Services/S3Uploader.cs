using AdImpressionService.Models;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Text.Json;

namespace AdImpressionService.Services
{
    public class S3Uploader
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "arity-ad-impressions";

        public S3Uploader(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task UploadAsync(AdImpression impression)
        {
            var transferUtility = new TransferUtility(_s3Client);
            var content = JsonSerializer.Serialize(impression);
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

            await transferUtility.UploadAsync(stream, _bucketName, $"{impression.ImpressionId}.json");
        }
    }
}
