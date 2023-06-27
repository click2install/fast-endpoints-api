using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FastEndApi.Features.Authorization.ApiKey;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IApiKeyService _service;

    public ApiKeyAuthenticationHandler(
      IOptionsMonitor<AuthenticationSchemeOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock,
      IApiKeyService service) : base(options, logger, encoder, clock)
    {
        Debug.Assert(service is not null);

        _service = service;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        var attribute = endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>();
        if (endpoint is null || attribute is not null)
        {
            return AuthenticateResult.NoResult();
        }

        if (!Request.Headers.TryGetValue(ApiKeyAuthenticationDefaults.HeaderName, out var header) ||
            string.IsNullOrWhiteSpace(header.First()))
        {
            return AuthenticateResult.Fail("Authorization header missing or empty.");
        }

        var apiKey = header.First()!;
        var result = await _service.ValidateAuthToken(apiKey, CancellationToken.None);

        if (!result.TryPickT0(out var auth, out _))
        {
            return AuthenticateResult.Fail("Authorization invalid.");
        }

        if (auth.TokenExpired)
        {
            return AuthenticateResult.Fail("Authorization expired.");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, auth.UserId.ToString())
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
