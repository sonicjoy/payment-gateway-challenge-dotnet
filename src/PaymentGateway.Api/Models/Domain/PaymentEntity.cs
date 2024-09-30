using System.Text.Json.Serialization;

using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.ValueTypes;

namespace PaymentGateway.Api.Models.Domain;

public record PaymentEntity
{
    public PaymentEntity(CardNumber cardNumber, int expiryMonth,
        int expiryYear, CurrencyEnum currency, int amount)
    {
        CardNumberLastFour = cardNumber.MaskedValue;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        Currency = currency;
        Amount = amount;
    }

    public Guid? Id { get; private set; }

    public PaymentStatus Status { get; private set; }
    public string? CardNumberLastFour { get; private set; }
    public int? ExpiryMonth { get; private set; }
    public int? ExpiryYear { get; private set; }
    public CurrencyEnum Currency { get; private set; }
    public int? Amount { get; private set; }

    public void SetId(Guid id)
    {
        Id ??= id;
    }

    public void SetStatus(PaymentStatus status)
    {
        Status = status;
    }
}
