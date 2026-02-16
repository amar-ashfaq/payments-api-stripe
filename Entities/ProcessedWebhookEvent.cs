namespace Payments.Entities
{
    public class ProcessedWebhookEvent
    {
        public long Id { get; set; }
        public string Provider { get; set; } = "Stripe";
        public string EventId { get; set; } = null!;
        public DateTime ProcessedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
