using System.Text.Json.Serialization;

using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Models.ValueTypes;

namespace PaymentGateway.Api.Models.PaymentService;

public record PaymentEntity
{
    public PaymentEntity(PaymentRequest paymentRequest)
    {
        CardNumberLastFour = ((CardNumber)paymentRequest.CardNumber).MaskedValue;
        ExpiryMonth = paymentRequest.ExpiryMonth;
        ExpiryYear = paymentRequest.ExpiryYear;
        Currency = paymentRequest.Currency;
        Amount = paymentRequest.Amount;
    }

    [JsonConstructor]
    public PaymentEntity(Guid id, PaymentStatus status, string cardNumberLastFour, int expiryMonth,
        int expiryYear, CurrencyEnum currency, int amount)
    {
        Id = id;
        Status = status;
        CardNumberLastFour = cardNumberLastFour;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        Currency = currency;
        Amount = amount;
    }

    public Guid? Id { get; private set; }

    public PaymentStatus Status { get; set; }
    public string? CardNumberLastFour { get; private set; }
    public int? ExpiryMonth { get; private set; }
    public int? ExpiryYear { get; private set; }
    public CurrencyEnum Currency { get; private set; }
    public int? Amount { get; private set; }

    public void SetId(Guid id)
    {
        Id ??= id;
    }
}
