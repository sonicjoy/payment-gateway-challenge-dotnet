using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Models.Domain;
using PaymentGateway.Api.Models.PaymentService;
using PaymentGateway.Api.Services.HttpClients;

namespace PaymentGateway.Api.Services;

public interface IPaymentService
{
    Task<PaymentEntity> ProcessPayment(PaymentRequest paymentRequest);
}

public class PaymentService(
    PaymentRequestValidator paymentRequestValidator,
    AcquiringBankClient httpClient,
    IPaymentsRepository paymentsRepository,
    ILogger<PaymentService> logger) : IPaymentService
{
    public async Task<PaymentEntity> ProcessPayment(PaymentRequest paymentRequest)
    {
        var validationResult = await paymentRequestValidator.ValidateAsync(paymentRequest);

        var entity = paymentRequest.CreatePaymentEntity();

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(e =>
                logger.LogWarning(
                    $"Validation error {e.ErrorCode} occured when process payment, message: {e.ErrorMessage}"));

            entity.SetStatus(PaymentStatus.Rejected);
            return entity;
        }


        var response = await httpClient.PostPaymentAsync(new AcquiringBankRequest(paymentRequest));

        entity.SetStatus(response.Authorized ? PaymentStatus.Authorized : PaymentStatus.Declined);

        await paymentsRepository.Add(entity);

        return entity;
    }
}