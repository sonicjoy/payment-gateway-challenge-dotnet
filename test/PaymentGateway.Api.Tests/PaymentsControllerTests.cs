using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Models.Controllers.Responses;
using PaymentGateway.Api.Models.DataStore;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Helpers;

namespace PaymentGateway.Api.Tests;

public class PaymentsControllerTests
{
    private readonly Random _random = new();

    private readonly HttpClient _client;
    private readonly PaymentsRepository _paymentsRepository;
    private readonly Mock<IPaymentService> _paymentServiceMock = new();

    public PaymentsControllerTests()
    {
        _paymentsRepository = new PaymentsRepository();

        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        _client = webApplicationFactory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IPaymentsRepository>(_paymentsRepository);
                    services.AddSingleton(_paymentServiceMock.Object);
                })).CreateClient();
        ;
    }

    [Fact]
    public async Task RetrievesAPaymentSuccessfully()
    {
        // Arrange
        var payment = new PaymentEntity(
            "12345678901234",
            _random.Next(1, 12),
            _random.Next(2023, 2030),
            CurrencyEnum.GBP,
            _random.Next(1, 10000)
        );
        await _paymentsRepository.Add(payment);

        // Act
        var response = await _client.GetAsync($"/api/payments/{payment.Id}");

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
        var response = await _client.GetAsync($"/api/payments/{Guid.NewGuid()}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostPaymentRequest_should_return_Ok200_with_RequestResponse_if_Authorized()
    {
        PaymentRequest request = new()
        {
            CardNumber = "12345678901234",
            ExpiryMonth = _random.Next(1, 12),
            ExpiryYear = _random.Next(2025, 2030),
            Currency = CurrencyEnum.GBP,
            Amount = _random.Next(1, 10000),
            Cvv = "123"
        };

        PaymentEntity paymentEntity = new(
            request.MaskedCardNumber,
            request.ExpiryMonth,
            request.ExpiryYear,
            request.Currency,
            request.Amount
        );

        paymentEntity.SetStatus(PaymentStatus.Authorized);

        _paymentServiceMock.Setup(s => s.ProcessPayment(request)).ReturnsAsync(paymentEntity);

        var response = await _client.PostAsJsonAsync("/api/payments", request, Config.GlobalJsonSerializerOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>(Config.GlobalJsonSerializerOptions);

        paymentResponse.Should().NotBeNull();

        paymentResponse.Status.Should().Be(PaymentStatus.Authorized);
 
    }

    [Fact]
    public async Task PostPaymentRequest_should_return_Created201_with_RequestResponse_if_Decline()
    {
        PaymentRequest request = new()
        {
            CardNumber = "12345678901234",
            ExpiryMonth = _random.Next(1, 12),
            ExpiryYear = _random.Next(2025, 2030),
            Currency = CurrencyEnum.GBP,
            Amount = _random.Next(1, 10000),
            Cvv = "123"
        };

        PaymentEntity paymentEntity = new(
            request.MaskedCardNumber,
            request.ExpiryMonth,
            request.ExpiryYear,
            request.Currency,
            request.Amount
        );

        paymentEntity.SetStatus(PaymentStatus.Declined);

        _paymentServiceMock.Setup(s => s.ProcessPayment(request)).ReturnsAsync(paymentEntity);

        var response = await _client.PostAsJsonAsync("/api/payments", request, Config.GlobalJsonSerializerOptions);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>(Config.GlobalJsonSerializerOptions);

        paymentResponse.Should().NotBeNull();

        paymentResponse.Status.Should().Be(PaymentStatus.Declined);
    }

    [Fact]
    public async Task PostPaymentRequest_should_return_NotAcceptable_with_RequestResponse_if_Rejected()
    {
        PaymentRequest request = new()
        {
            CardNumber = "12345678as901234",
            ExpiryMonth = _random.Next(1, 12),
            ExpiryYear = _random.Next(2015, 2030),
            Currency = CurrencyEnum.GBP,
            Amount = _random.Next(1, 10000),
            Cvv = "123"
        };

        PaymentEntity paymentEntity = new(
            request.MaskedCardNumber,
            request.ExpiryMonth,
            request.ExpiryYear,
            request.Currency,
            request.Amount
        );

        paymentEntity.SetStatus(PaymentStatus.Rejected);

        _paymentServiceMock.Setup(s => s.ProcessPayment(request)).ReturnsAsync(paymentEntity);

        var response = await _client.PostAsJsonAsync("/api/payments", request, Config.GlobalJsonSerializerOptions);

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
        
        var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>(Config.GlobalJsonSerializerOptions);

        paymentResponse.Should().NotBeNull();

        paymentResponse.Status.Should().Be(PaymentStatus.Rejected);
    }
}