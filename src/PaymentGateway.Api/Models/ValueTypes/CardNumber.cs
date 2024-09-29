using System.Text.Json;
using System.Text.Json.Serialization;

using FluentValidation;

namespace PaymentGateway.Api.Models.ValueTypes;

public record struct CardNumber(string Value)
{
    public string MaskedValue => string.Concat(new string('*', Value.Length - 4), Value.AsSpan(Value.Length - 4));
}

public class CardNumberJsonConverter : JsonConverter<CardNumber>
{
    public override CardNumber Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new CardNumber(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, CardNumber cardNumber, JsonSerializerOptions options)
    {
        writer.WriteStringValue(cardNumber.Value);
    }
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