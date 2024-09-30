using System.Text.Json;
using System.Text.Json.Serialization;
namespace PaymentGateway.Api.Services.Helpers;

public static class Config
{
    public static JsonSerializerOptions GlobalJsonSerializerOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter() }
    };
}