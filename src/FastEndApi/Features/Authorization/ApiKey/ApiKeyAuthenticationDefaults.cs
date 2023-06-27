
namespace FastEndApi.Features.Authorization.ApiKey;

public readonly record struct ApiKeyAuthenticationDefaults
{
    public const string AuthenticationScheme = "ApiKey";

    public const string HeaderName = "X-Api-Key";
}
