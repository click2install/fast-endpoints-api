using System.Diagnostics;

using FluentValidation;

using Microsoft.Extensions.Options;

namespace FastEndApi.Extensions;

public static class OptionsBuilderDataAnnotationsExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluent<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services
          .AddSingleton<IValidateOptions<TOptions>>(svc =>
          {
              var validator = svc.GetRequiredService<IValidator<TOptions>>();
              var options = new FluentValidationOptions<TOptions>(optionsBuilder.Name, validator);

              return options;
          });

        return optionsBuilder;
    }

    private class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        public string? Name { get; }
        private readonly IValidator<TOptions> _validator;

        public FluentValidationOptions(string? name, IValidator<TOptions> validator)
        {
            Name = name;
            _validator = validator;
        }

        public ValidateOptionsResult Validate(string? name, TOptions options)
        {
            Debug.Assert(options is not null);

            if (Name is not null && Name != name)
            {
                return ValidateOptionsResult.Skip;
            }

            var result = _validator.Validate(options);
            if (result.IsValid)
            {
                return ValidateOptionsResult.Success;
            }

            var errors = result.Errors
              .Select(x => $"Options validation failed for '{x.PropertyName} with error: {x.ErrorMessage}.");

            return ValidateOptionsResult.Fail(errors);
        }
    }
}
