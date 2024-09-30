using System.Net;

using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Enums;
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

        return new OkObjectResult(payment); //implicitly cast PaymentEntity to PaymentResponse
    }

    [HttpPost]
    public async Task<ActionResult<PaymentResponse>> PostPaymentAsync(PaymentRequest paymentRequest)
    {
        var response = await paymentService.ProcessPayment(paymentRequest);

        return (response.Status) switch
        {
            PaymentStatus.Authorized => Ok(response),
            PaymentStatus.Declined => StatusCode(StatusCodes.Status201Created, response),
            PaymentStatus.Rejected => StatusCode(StatusCodes.Status406NotAcceptable, response),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}