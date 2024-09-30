using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Models.DataStore;
using PaymentGateway.Api.Models.PaymentService;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Helpers;
using PaymentGateway.Api.Services.HttpClients;

namespace PaymentGateway.Api.Tests;

public class PaymentServiceTests
{
    private readonly Mock<IAcquiringBankClient> _httpClient = new();
    private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock = new();
    private readonly Mock<ILogger<PaymentService>> _loggerMock = new();
    private readonly PaymentService _paymentService;

    public PaymentServiceTests()
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider.Setup(d => d.Now).Returns(new DateTime(2024, 9, 30));

        PaymentRequestValidator validator = new(dateTimeProvider.Object);
        _paymentService = new PaymentService(validator, _httpClient.Object, _paymentsRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ProcessPayment_should_validate_PaymentRequest_and_set_Rejected_status_if_failed()
    {
        //arrange
        var paymentRequest = new PaymentRequest
        {
            CardNumber = "12345678ss901234", // invalid card number
            ExpiryMonth = 12,
            ExpiryYear = 2023,
            Currency = CurrencyEnum.GBP,
            Amount = 100
        };

        //act
        var entity = await _paymentService.ProcessPayment(paymentRequest);

        //assert
        entity.Should().NotBeNull();

        entity.Status.Should().Be(PaymentStatus.Rejected);
    }

    [Fact]
    public async Task ProcessPayment_should_call_httpClient_after_validating_PaymentRequest()
    {
        //arrange
        var paymentRequest = new PaymentRequest
        {
            CardNumber = "12345678901234",
            ExpiryMonth = 12,
            ExpiryYear = 2024,
            Currency = CurrencyEnum.GBP,
            Amount = 100
        };

        //act
        var entity = await _paymentService.ProcessPayment(paymentRequest);

        //assert
        entity.Should().NotBeNull();

        _httpClient.Verify(h => h.PostPaymentAsync(It.IsAny<AcquiringBankRequest>()), Times.Once);
    }

    [Fact]
    public async Task ProcessPayment_should_store_PaymentEntity_with_masked_card_number_after_receiving_Authorized_response()
    {
        //arrange
        var paymentRequest = new PaymentRequest
        {
            CardNumber = "12345678901234",
            ExpiryMonth = 12,
            ExpiryYear = 2024,
            Currency = CurrencyEnum.GBP,
            Amount = 100
        };

        //act
        _httpClient.Setup(c => c.PostPaymentAsync(It.IsAny<AcquiringBankRequest>()))
            .ReturnsAsync(new AcquiringBankResponse(true, Guid.NewGuid().ToString()));

        var id = Guid.NewGuid();
        _paymentsRepositoryMock.Setup(h => h.Add(It.IsAny<PaymentEntity>()))
            .Callback<PaymentEntity>(entity =>
            {
                entity.SetId(id);
            });

        var entity = await _paymentService.ProcessPayment(paymentRequest);

        //assert
        _httpClient.Verify(c => c.PostPaymentAsync(It.IsAny<AcquiringBankRequest>()), Times.Once);
        _paymentsRepositoryMock.Verify(h => h.Add(It.IsAny<PaymentEntity>()), Times.Once);
        
        entity.Should().NotBeNull();

        entity.Id.Should().Be(id);

        entity.CardNumberLastFour.Should().Be("************1234");

        entity.Status.Should().Be(PaymentStatus.Authorized);

        entity.AuthorizationCode.Should().NotBeNull();
    }

    [Fact]
    public async Task ProcessPayment_should_store_PaymentEntity_with_masked_card_number_after_receiving_Declined_response()
    {
        //arrange
        var paymentRequest = new PaymentRequest
        {
            CardNumber = "12345678901234",
            ExpiryMonth = 12,
            ExpiryYear = 2024,
            Currency = CurrencyEnum.GBP,
            Amount = 100
        };

        //act
        _httpClient.Setup(c => c.PostPaymentAsync(It.IsAny<AcquiringBankRequest>()))
            .ReturnsAsync(new AcquiringBankResponse(false, string.Empty));

        var id = Guid.NewGuid();
        _paymentsRepositoryMock.Setup(h => h.Add(It.IsAny<PaymentEntity>()))
            .Callback<PaymentEntity>(entity =>
            {
                entity.SetId(id);
            });

        var entity = await _paymentService.ProcessPayment(paymentRequest);

        //assert
        _httpClient.Verify(c => c.PostPaymentAsync(It.IsAny<AcquiringBankRequest>()), Times.Once);
        _paymentsRepositoryMock.Verify(h => h.Add(It.IsAny<PaymentEntity>()), Times.Once);
        
        entity.Should().NotBeNull();

        entity.Id.Should().Be(id);

        entity.CardNumberLastFour.Should().Be("************1234");

        entity.Status.Should().Be(PaymentStatus.Declined);

    }
}