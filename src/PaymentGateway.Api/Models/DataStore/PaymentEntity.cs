using PaymentGateway.Api.Enums;

namespace PaymentGateway.Api.Models.DataStore;

public record PaymentEntity(
    string CardNumberLastFour,
    int ExpiryMonth,
    int ExpiryYear,
    CurrencyEnum Currency,
    int Amount)
{
    public Guid? Id { get; private set; }

    public PaymentStatus Status { get; private set; }

    public void SetId(Guid id)
    {
        Id ??= id;
    }

    public void SetStatus(PaymentStatus status)
    {
        Status = status;
    }

    public string? AuthorizationCode { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
