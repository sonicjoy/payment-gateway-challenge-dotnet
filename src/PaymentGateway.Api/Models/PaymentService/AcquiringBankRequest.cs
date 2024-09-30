using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;

namespace PaymentGateway.Api.Models.PaymentService;

public record struct AcquiringBankRequest(string CardNumber, string ExpiryDate, CurrencyEnum Currency, int Amount, string Cvv)
{
    public AcquiringBankRequest(PaymentRequest paymentRequest)
        : this(paymentRequest.CardNumber,
            paymentRequest.ExpiryDate.ToString("MM/yyyy"),
            paymentRequest.Currency,
            paymentRequest.Amount,
            paymentRequest.Cvv)
    {
    }
}