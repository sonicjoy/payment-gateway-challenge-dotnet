﻿namespace PaymentGateway.Api.Models.ValueTypes;

public static class CardNumberHelper
{
    public static string MaskedValue(string cardNumber) => 
        string.Concat(new string('*', cardNumber.Length - 4),
        cardNumber.AsSpan(cardNumber.Length - 4));
}