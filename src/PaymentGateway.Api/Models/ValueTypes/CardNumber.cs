namespace PaymentGateway.Api.Models.ValueTypes;

public record struct CardNumber(string Value)
{
    public string MaskedValue => string.Concat(new string('*', Value.Length - 4), Value.AsSpan(Value.Length - 4));

    public static implicit operator CardNumber(string cardNumber) => new(cardNumber);

    public static implicit operator string(CardNumber cardNumber) => cardNumber.Value;
}