using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Services.Helpers;

namespace PaymentGateway.Api.Models.PaymentService;

public record struct AcquiringBankRequest(string CardNumber, string ExpiryDate, CurrencyEnum Currency, int Amount, string Cvv)
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