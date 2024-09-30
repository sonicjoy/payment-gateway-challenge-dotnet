using FluentValidation.TestHelper;

using Moq;

using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Models.ValueTypes;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Helpers;

namespace PaymentGateway.Api.Tests;

public class PaymentRequestValidatorTests
{
    private readonly PaymentRequestValidator _validator;

    public PaymentRequestValidatorTests()
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider.Setup(d => d.Now).Returns(new DateTime(2024, 9, 30));
        _validator = new PaymentRequestValidator(dateTimeProvider.Object);
    }


    [Fact]
    public void CardNumber_should_be_14_to_19_digits()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            1,
            2025,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldNotHaveValidationErrorFor(p => p.CardNumber);
    }

    [Fact]
    public void CardNumber_should_have_validation_error_as_it_has_20_digits()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234567890",
            1,
            2025,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldHaveValidationErrorFor(p => p.CardNumber);
    }

    [Fact]
    public void CardNumber_should_have_validation_error_as_it_has_10_digits()
    {
        var paymentRequest = new PaymentRequest
        (
            "1234567890",
            1,
            2025,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldHaveValidationErrorFor(p => p.CardNumber);
    }

    [Fact]
    public void CardNumber_should_have_validation_error_as_it_contains_non_numeric_characters()
    {
        var paymentRequest = new PaymentRequest
        (
            "1234567890123a",
            1,
            2025,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldHaveValidationErrorFor(p => p.CardNumber);
    }

    [Fact]
    public void ExpiryMonth_should_be_between_1_to_12()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            1,
            2025,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldNotHaveValidationErrorFor(p => p.ExpiryMonth);
    }

    [Fact]
    public void ExpiryMonth_should_have_validation_error_as_it_is_0()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            0,
            2025,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldHaveValidationErrorFor(p => p.ExpiryMonth);
    }

    [Fact]
    public void ExpiryYear_should_have_validation_error_as_it_is_in_the_past()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            9,
            2023,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldHaveValidationErrorFor(p => p.ExpiryYear);
    }

    [Fact]
    public void ExpiryYear_should_not_have_validation_error_as_it_is_in_the_future()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            9,
            2025,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldNotHaveValidationErrorFor(p => p.ExpiryYear);
    }

    [Fact]
    public void ExpiryDate_should_have_validation_error_as_it_is_not_in_the_future()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            9,
            2024,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldHaveValidationErrorFor("ExpiryDate");
    }

    [Fact]
    public void ExpiryDate_should_not_have_validation_error_as_it_is_in_the_future()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            12,
            2024,
            CurrencyEnum.USD,
            100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldNotHaveValidationErrorFor("ExpiryDate");
    }

    [Fact]
    public void Amount_should_have_validation_error_when_it_is_negative()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            12,
            2024,
            CurrencyEnum.USD,
            -100,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldHaveValidationErrorFor(p => p.Amount);
    }

    [Fact]
    public void Amount_should_not_have_validation_error_when_it_is_not_negative()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            12,
            2024,
            CurrencyEnum.USD,
            0,
            "123"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldNotHaveValidationErrorFor(p => p.Amount);
    }

    [Fact]
    public void Cvv_should_have_validation_error_as_it_is_not_3_to_4_digits_long()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            12,
            2024,
            CurrencyEnum.USD,
            100,
            "12345"
        );

        var result = _validator.TestValidate(paymentRequest);
        result.ShouldHaveValidationErrorFor(p => p.Cvv);
    }

    [Fact]
    public void Cvv_should_not_have_validation_error_as_it_is_3_digits_long()
    {
        var paymentRequest = new PaymentRequest
        (
            "12345678901234",
            12,
            2024,
            CurrencyEnum.USD,
            100,
            "123"
        );
        var result = _validator.TestValidate(paymentRequest);
        result.ShouldNotHaveValidationErrorFor(p => p.Cvv);
    }
}