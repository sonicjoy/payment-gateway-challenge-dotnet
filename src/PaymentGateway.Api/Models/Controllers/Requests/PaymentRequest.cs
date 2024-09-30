using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.ValueTypes;

namespace PaymentGateway.Api.Models.Controllers.Requests;

public record PaymentRequest(
    CardNumber CardNumber,
    int ExpiryMonth,
    int ExpiryYear,
    CurrencyEnum Currency,
    int Amount,
    Cvv Cvv
);