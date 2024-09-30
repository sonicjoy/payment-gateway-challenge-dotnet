using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Models.Controllers.Responses;
using PaymentGateway.Api.Models.Domain;
using PaymentGateway.Api.Models.PaymentService;
using PaymentGateway.Api.Models.ValueTypes;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Helpers;

namespace PaymentGateway.Api.Tests;

public class PaymentsControllerTests
{
    private readonly Random _random = new();

    private readonly HttpClient _client;
    private readonly PaymentsRepository _paymentsRepository;

    public PaymentsControllerTests()
    {
        _paymentsRepository = new PaymentsRepository();

        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        _client = webApplicationFactory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services => ((ServiceCollection)services)
                    .AddSingleton<IPaymentsRepository>(_paymentsRepository)))
            .CreateClient();
    }

    [Fact]
    public async Task RetrievesAPaymentSuccessfully()
    {
        // Arrange
        var payment = new PaymentEntity(
            new CardNumber("12345678901234"),
            _random.Next(1, 12),
            _random.Next(2023, 2030),
            CurrencyEnum.GBP,
            _random.Next(1, 10000)
        );
        await _paymentsRepository.Add(payment);

        // Act
        var response = await _client.GetAsync($"/api/Payments/{payment.Id}");

        var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>(Config.GlobalJsonSerializerOptions);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        paymentResponse.Should().NotBeNull();
    }

    [Fact]
    public async Task Returns404IfPaymentNotFound()
    {
        // Arrange
       
        // Act
        var response = await _client.GetAsync($"/api/Payments/{Guid.NewGuid()}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    
}