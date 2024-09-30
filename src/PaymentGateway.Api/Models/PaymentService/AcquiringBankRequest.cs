using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Models.ValueTypes;
using PaymentGateway.Api.Services.Helpers;

namespace PaymentGateway.Api.Models.PaymentService;

public record struct AcquiringBankRequest(CardNumber CardNumber, string ExpiryDate, CurrencyEnum Currency, int Amount, Cvv Cvv)
{
    public AcquiringBankRequest(PaymentRequest paymentRequest)
        : this(paymentRequest.CardNumber,
            ExpiryDateHelper.ExpiryDate(paymentRequest.ExpiryMonth, paymentRequest.ExpiryYear),
            paymentRequest.Currency,
            paymentRequest.Amount,
            paymentRequest.Cvv)
    {
    }
}