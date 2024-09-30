using PaymentGateway.Api.Enums;

namespace PaymentGateway.Api.Models.Controllers.Requests;

public record struct PaymentRequest(
    string CardNumber,
    int ExpiryMonth,
    int ExpiryYear,
    CurrencyEnum Currency,
    int Amount,
    string Cvv
)
{
    public DateTime ExpiryDate
    {
        get
        {
            try
            {
                // The expiry date on your debit card is based on your local time.
                // You can use your card until 11:59 PM on the last day of the month indicated on your card.
                DateTime expiryDate = new(ExpiryYear, ExpiryMonth, DateTime.DaysInMonth(ExpiryYear, ExpiryMonth));

                return expiryDate.AddDays(1);
            }
            catch (ArgumentOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }
    }

    // only show the last 4 digits of the card number
    public string CardNumberLastFour => CardNumber[^4..];
}