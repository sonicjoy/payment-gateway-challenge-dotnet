using System.Text.Json.Serialization;
using System.Text.Json;

namespace PaymentGateway.Api.Models.ValueTypes;

public interface INumericString
{
    string Value { get; }
}
