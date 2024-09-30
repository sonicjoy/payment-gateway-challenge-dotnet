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
                DateTime expiryDate = new(ExpiryYear, ExpiryMonth, 1);

                return expiryDate;
            }
            catch (ArgumentOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }
    }

    // Mask the card number to only show the last 4 digits and regardless the real length of card number shows 12 '*' characters
    public string MaskedCardNumber => string.Concat(new string('*', 12), CardNumber.AsSpan(CardNumber.Length - 4));
}