namespace PaymentGateway.Api.Services.Helpers;

public static class ExpiryDateHelper
{
    public static string ExpiryDate(int expiryMonth, int expiryYear)
    {
        try
        {
            DateTime expiryDate = new(expiryYear, expiryMonth, 1);

            return expiryDate.ToString("MM/yyyy");
        }
        catch (ArgumentOutOfRangeException)
        {
            return "00/0000";
        }
    }
}