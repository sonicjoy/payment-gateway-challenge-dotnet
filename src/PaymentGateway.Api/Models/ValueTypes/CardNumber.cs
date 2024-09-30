using FluentValidation;

namespace PaymentGateway.Api.Models.ValueTypes;

public record struct CardNumber(string Value) : INumericString
{
    public string MaskedValue => string.Concat(new string('*', Value.Length - 4), Value.AsSpan(Value.Length - 4));

    public static implicit operator CardNumber(string cardNumber) => new(cardNumber);

    public static implicit operator string(CardNumber cardNumber) => cardNumber.Value;
}

public class CardNumberValidator : AbstractValidator<CardNumber>
{
    public CardNumberValidator()
    {
        RuleFor(x => x.Value)
            .Matches(@"^\d{14,19}$")
            .WithMessage("Card number must be between 14 and 19 digits");
    }
}