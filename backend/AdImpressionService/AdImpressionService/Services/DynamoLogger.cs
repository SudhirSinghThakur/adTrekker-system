using AdImpressionService.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace AdImpressionService.Services
{
    public class DynamoLogger
    {
        private readonly AmazonDynamoDBClient _dynamoDbClient;
        private readonly Table _table;

        public DynamoLogger()
        {
            _dynamoDbClient = new AmazonDynamoDBClient();
            var tableBuilder = new TableBuilder(_dynamoDbClient, "AdImpressionMetadata");
            _table = (Table)tableBuilder.Build();
        }

        public async Task LogAsync(AdImpression impression)
        {
            var doc = new Document
            {
                ["ImpressionId"] = impression.ImpressionId,
                ["CampaignId"] = impression.CampaignId,
                ["Timestamp"] = impression.Timestamp.ToString("o"),
                ["Location"] = impression.Location
            };

            await _table.PutItemAsync(doc);
        }

        public async Task<List<AdImpression>> GetAllImpressionsAsync()
        {
            var results = new List<AdImpression>();
            var scanConfig = new ScanOperationConfig();
            var search = _table.Scan(scanConfig);

            do
            {
                var documents = await search.GetNextSetAsync();
                foreach (var doc in documents)
                {
                    results.Add(new AdImpression
                    {
                        ImpressionId = doc["ImpressionId"],
                        CampaignId = doc["CampaignId"],
                        Timestamp = DateTime.Parse(doc["Timestamp"]),
                        Location = doc.TryGetValue("Location", out var location) ? location : "N/A"
                    });
                }
            } while (!search.IsDone);

            return results;
        }

    }
}
