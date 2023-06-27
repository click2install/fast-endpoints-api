
using FluentValidation;

namespace FastEndApi;

public record AppConfiguration
{
    public ConnectionStrings ConnectionStrings { get; init; } = default!;
}

public record ConnectionStrings
{
    public string DefaultConnection { get; init; } = default!;
}

public class AppConfigurationValidator : AbstractValidator<AppConfiguration>
{
    public AppConfigurationValidator()
    {
        RuleFor(x => x.ConnectionStrings)
          .Cascade(CascadeMode.Stop)
          .NotNull()
          .Must((_, x) => !string.IsNullOrWhiteSpace(x.DefaultConnection));
    }
}
