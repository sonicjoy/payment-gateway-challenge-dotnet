using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Domain;
using PaymentGateway.Api.Models.ValueTypes;

namespace PaymentGateway.Api.Models.Controllers.Requests;

public record PaymentRequest(
    string CardNumber,
    int ExpiryMonth,
    int ExpiryYear,
    CurrencyEnum Currency,
    int Amount,
    string Cvv
)
{
    public PaymentEntity CreatePaymentEntity() => new(CardNumber, ExpiryMonth, ExpiryYear, Currency, Amount);
};