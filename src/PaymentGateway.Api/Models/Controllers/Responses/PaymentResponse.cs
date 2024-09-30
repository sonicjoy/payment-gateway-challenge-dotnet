using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.DataStore;

namespace PaymentGateway.Api.Models.Controllers.Responses;

public record struct PaymentResponse
{
    public PaymentResponse(PaymentEntity paymentEntity)
    {
        Id = paymentEntity.Id;
        Status = paymentEntity.Status;
        CardNumberLastFour = paymentEntity.CardNumberLastFour;
        ExpiryMonth = paymentEntity.ExpiryMonth;
        ExpiryYear = paymentEntity.ExpiryYear;
        Currency = paymentEntity.Currency;
        Amount = paymentEntity.Amount;
        CreatedAt = paymentEntity.CreatedAt;
    }

    public Guid? Id { get; set; }
    public PaymentStatus Status { get; set; }
    public string? CardNumberLastFour { get; set; }
    public int? ExpiryMonth { get; set; }
    public int? ExpiryYear { get; set; }
    public CurrencyEnum Currency { get; set; }
    public int? Amount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}