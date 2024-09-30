namespace PaymentGateway.Api.Models.PaymentService;

public record struct AcquiringBankResponse(bool Authorized, string AuthorizationCode);