using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Models.DataStore;
using PaymentGateway.Api.Models.PaymentService;
using PaymentGateway.Api.Services.HttpClients;

namespace PaymentGateway.Api.Services;

public interface IPaymentService
{
    Task<PaymentEntity> ProcessPayment(PaymentRequest paymentRequest);
}

public class PaymentService(
    PaymentRequestValidator paymentRequestValidator,
    IAcquiringBankClient httpClient,
    IPaymentsRepository paymentsRepository,
    ILogger<PaymentService> logger) : IPaymentService
{
    public async Task<PaymentEntity> ProcessPayment(PaymentRequest paymentRequest)
    {
        var validationResult = await paymentRequestValidator.ValidateAsync(paymentRequest);

        var entity = new PaymentEntity(
            paymentRequest.MaskedCardNumber,
            paymentRequest.ExpiryMonth, paymentRequest.ExpiryYear,
            paymentRequest.Currency, paymentRequest.Amount);

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(e =>
                logger.LogWarning(
                    $"Validation error {e.ErrorCode} occured when process payment, message: {e.ErrorMessage}"));

            entity.SetStatus(PaymentStatus.Rejected);
            return entity;
        }

        var response = await httpClient.PostPaymentAsync(new AcquiringBankRequest(paymentRequest));

        if (response.Authorized)
        {
            logger.LogInformation($"Payment authorized for {entity.CardNumberLastFour}, authorization code {response.AuthorizationCode}.");
            entity.SetStatus(PaymentStatus.Authorized);
            entity.AuthorizationCode = response.AuthorizationCode;
        }
        else
        {
            logger.LogInformation($"Payment declined for {entity.CardNumberLastFour}.");
            entity.SetStatus(PaymentStatus.Declined);
        }

        await paymentsRepository.Add(entity);

        return entity;
    }
}