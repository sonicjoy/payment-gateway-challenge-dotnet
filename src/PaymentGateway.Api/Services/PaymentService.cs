using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
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

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(e =>
                logger.LogWarning(
                    $"Validation error {e.ErrorCode} occured when process payment, message: {e.ErrorMessage}"));

            return new PaymentEntity(paymentRequest) { Status = PaymentStatus.Rejected };
        }


        var response = await httpClient.PostPaymentAsync(new AcquiringBankRequest(paymentRequest));

        var paymentResponse = new PaymentEntity(paymentRequest) { Status = response.Authorized ? PaymentStatus.Authorized : PaymentStatus.Declined };

        await paymentsRepository.Add(paymentResponse);

        return paymentResponse;
    }
}