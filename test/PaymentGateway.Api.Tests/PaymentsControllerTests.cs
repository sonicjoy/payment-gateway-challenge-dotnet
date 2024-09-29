using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Responses;
using PaymentGateway.Api.Models.ValueTypes;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Tests;

public class PaymentsControllerTests
{
    private readonly Random _random = new();

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private readonly HttpClient _client;
    private readonly PaymentsRepository _paymentsRepository;

    public PaymentsControllerTests()
    {
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        _jsonSerializerOptions.Converters.Add(new CardNumberJsonConverter());

        _paymentsRepository = new PaymentsRepository();

        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        _client = webApplicationFactory.WithWebHostBuilder(builder =>
                builder.ConfigureServices(services => ((ServiceCollection)services)
                    .AddSingleton(_paymentsRepository)))
            .CreateClient();
    }

    [Fact]
    public async Task RetrievesAPaymentSuccessfully()
    {
        // Arrange
        var payment = new PostPaymentResponse
        {
            Id = Guid.NewGuid(),
            ExpiryYear = _random.Next(2023, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1, 10000),
            CardNumberLastFour = _random.Next(1111, 9999),
            Currency = "GBP",
        };
        _paymentsRepository.Add(payment);

        // Act
        var response = await _client.GetAsync($"/api/Payments/{payment.Id}");
        var paymentResponse = await response.Content.ReadFromJsonAsync<PostPaymentResponse>(_jsonSerializerOptions);
        
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