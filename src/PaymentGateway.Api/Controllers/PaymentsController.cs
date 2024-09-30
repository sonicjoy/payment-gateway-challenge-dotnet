using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Models.Controllers.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController(IPaymentsRepository paymentsRepository, IPaymentService paymentService) : Controller
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PaymentResponse?>> GetPaymentAsync(Guid id)
    {
        var payment = await paymentsRepository.Get(id);
        
        if (payment is null)
        {
            return NotFound();
        }

        return new OkObjectResult(payment);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentResponse>> PostPaymentAsync(PaymentRequest paymentRequest)
    {
        var response = await paymentService.ProcessPayment(paymentRequest);
        return new CreatedResult($"/api/payments/{response.Id}", response);
    }
}