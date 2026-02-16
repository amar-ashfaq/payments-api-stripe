namespace Payments.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? StripePaymentIntentId { get; set; }
        public long Amount { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
