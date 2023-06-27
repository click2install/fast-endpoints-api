using FluentValidation;

namespace FastEndApi.Features.Account.Login;

public record LoginRequest
{
    public string UserName { get; init; } = default!;

    public string Password { get; init; } = default!;
}

public class Validator : Validator<LoginRequest>
{
    public Validator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(5);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(10);
    }
}
