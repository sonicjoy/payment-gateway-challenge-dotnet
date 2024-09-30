using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.ValueTypes;

namespace PaymentGateway.Api.Models.Domain;

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
}
