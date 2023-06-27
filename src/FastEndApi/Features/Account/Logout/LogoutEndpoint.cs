using System.Diagnostics;
using System.Security.Claims;

using FastEndApi.Extensions;

namespace FastEndApi.Features.Account.Logout;

public class LogoutEndpoint : EndpointWithoutRequest
{
    private readonly ILogoutService _service;

    public LogoutEndpoint(ILogoutService service)
    {
        Debug.Assert(service is not null);

        _service = service;
    }

    public override void Configure()
    {
        Post("/account/logout");

        Summary(s =>
        {
            s.Summary = "Logs a user out of their account.";
            s.Description = "Logs the user out of their account and invalidates their API key.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

        var result = await _service.Logout(Guid.Parse(id), cancellationToken);
        if (result.IsT0)
        {
            await SendNoContentAsync(cancellationToken);
            return;
        }

        this.AddGeneralError("Logout failed.");
        await SendErrorsAsync(cancellation: cancellationToken);
    }
}