namespace AdImpressionService.Models
{
    public class AdImpression
    {
        public string ImpressionId { get; set; }
        public string CampaignId { get; set; }
        public string DriverId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }
    }
}
