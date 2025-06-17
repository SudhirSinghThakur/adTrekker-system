using AdImpressionService.Models;
using AdImpressionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdImpressionService.Controllers
{
    [ApiController]
    [Route("api/v1/impressions")]
    public class AdImpressionController : ControllerBase
    {
        private readonly S3Uploader _s3Uploader;
        private readonly DynamoLogger _dynamoLogger;

        public AdImpressionController(S3Uploader s3Uploader, DynamoLogger dynamoLogger)
        {
            _s3Uploader = s3Uploader;
            _dynamoLogger = dynamoLogger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AdImpression impression)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _s3Uploader.UploadAsync(impression);
            await _dynamoLogger.LogAsync(impression);

            return Ok(new { message = "Impression recorded." });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var records = await _dynamoLogger.GetAllImpressionsAsync();
            return Ok(records);
        }

    }
}
