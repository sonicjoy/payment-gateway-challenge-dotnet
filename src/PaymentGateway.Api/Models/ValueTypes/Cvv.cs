using FluentValidation;

namespace PaymentGateway.Api.Models.ValueTypes;

public record struct Cvv(string Value) : INumericString
{
    public static implicit operator Cvv(string cvv) => new(cvv);

    public static implicit operator string(Cvv cvv) => cvv.Value;
};

public class CvvValidator : AbstractValidator<Cvv>
{
    public CvvValidator()
    {
        RuleFor(x => x.Value)
            .Matches(@"^\d{3,4}$")
            .WithMessage("Cvv must be between 3 and 4 digits");
    }
}