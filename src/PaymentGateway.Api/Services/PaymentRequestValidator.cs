﻿using FluentValidation;

using PaymentGateway.Api.Models.Controllers.Requests;
using PaymentGateway.Api.Services.Helpers;

namespace PaymentGateway.Api.Services;

public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
    public PaymentRequestValidator(IDateTimeProvider dateTimeProvider)
    {
        RuleFor(p => p.CardNumber)
            .Matches(@"^\d{14,19}$")
            .WithMessage("Card number must be between 14 and 19 digits");;

        RuleFor(p => p.ExpiryMonth).InclusiveBetween(1, 12);

        RuleFor(p => p.ExpiryYear).GreaterThanOrEqualTo(dateTimeProvider.Now.Date.Year);

        RuleFor(p => p).Custom((p, context) =>
        {
            try
            {
                DateTime expiryDate = new(p.ExpiryYear, p.ExpiryMonth, 1);

                if (expiryDate <= dateTimeProvider.Now.Date)
                {
                    context.AddFailure("ExpiryDate", "Expiry date is not in the future");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                context.AddFailure("ExpiryDate", "Invalid expiry date");
            }
        });

        RuleFor(p => p.Amount).GreaterThanOrEqualTo(0);

        RuleFor(p => p.Cvv)
            .Matches(@"^\d{3,4}$")
            .WithMessage("Cvv must be between 3 and 4 digits");;

    }
}