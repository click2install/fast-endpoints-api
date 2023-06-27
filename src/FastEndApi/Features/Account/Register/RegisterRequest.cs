using FluentValidation;

namespace FastEndApi.Features.Account.Register;

public record RegisterRequest
{
    public string UserName { get; init; } = default!;

    public string Password { get; init; } = default!;
}

public class RegisterValidator : Validator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(5);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(10);
    }
}
