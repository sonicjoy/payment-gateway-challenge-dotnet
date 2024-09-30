using System.Text;
using System.Text.Json;

using PaymentGateway.Api.Models.PaymentService;
using PaymentGateway.Api.Services.Helpers;

namespace PaymentGateway.Api.Services.HttpClients;

public interface IAcquiringBankClient
{
    Task<AcquiringBankResponse> PostPaymentAsync(AcquiringBankRequest request);
}

public class AcquiringBankClient(HttpClient client, ILogger<AcquiringBankClient> logger) : IAcquiringBankClient, IDisposable
{
    public async Task<AcquiringBankResponse> PostPaymentAsync(AcquiringBankRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, Config.GlobalJsonSerializerOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/payments", data);

            var paymentResponse = await response.Content.ReadFromJsonAsync<AcquiringBankResponse>(Config.GlobalJsonSerializerOptions);

            return paymentResponse;
        }
        catch (Exception ex)
        {
            logger.LogError("Error post payment request: {Error}", ex);
        }

        return new AcquiringBankResponse(false, string.Empty);
    }

    public void Dispose() => client.Dispose();
}