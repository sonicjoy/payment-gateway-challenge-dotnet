namespace PaymentGateway.Api.Services.Helpers;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTime.Now;
}