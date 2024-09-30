namespace PaymentGateway.Api.Enums;

public enum PaymentStatus
{
    /// <summary>
    /// Default status.
    /// No payment could be created as invalid information was supplied to the payment gateway,
    /// and therefore it has rejected the request without calling the acquiring bank.
    /// </summary>
    Rejected, 
    /// <summary>
    /// The payment was authorized by the call to the acquiring bank
    /// </summary>
    Authorized,
    /// <summary>
    /// The payment was declined by the call to the acquiring bank
    /// </summary>
    Declined,
}