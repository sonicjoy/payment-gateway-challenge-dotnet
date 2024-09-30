﻿using PaymentGateway.Api.Enums;

namespace PaymentGateway.Api.Models.Controllers.Responses;

public record struct PaymentResponse
{
    public Guid? Id { get; set; }
    public PaymentStatus Status { get; set; }
    public string? CardNumberLastFour { get; set; }
    public int? ExpiryMonth { get; set; }
    public int? ExpiryYear { get; set; }
    public string? Currency { get; set; }
    public int? Amount { get; set; }
}