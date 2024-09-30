using PaymentGateway.Api.Enums;

namespace PaymentGateway.Api.Models.Controllers.Requests;

public record struct PaymentRequest(
    string CardNumber,
    int ExpiryMonth,
    int ExpiryYear,
    CurrencyEnum Currency,
    int Amount,
    string Cvv
);