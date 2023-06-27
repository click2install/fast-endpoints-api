using System.Diagnostics;

using FastEndApi.Extensions;

namespace FastEndApi.Features.Account.Login;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse, LoginMapper>
{
    private readonly ILoginService _service;

    public LoginEndpoint(ILoginService service)
    {
        Debug.Assert(service is not null);

        _service = service;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Post("/account/login");

        Summary(s =>
        {
            s.Summary = "Logs a user into their account.";
            s.Description = @"Once authenticated the response will provide the user with their API key. The API key must be used
                              in all subsequent requests as the value of the `X-Api-Token` request header. Failure to include the API key
                              for authenticated requests will result in a HTTP 401 status.";
        });
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.Login(request.UserName, request.Password, cancellationToken);
        if (result.IsT0)
        {
            var response = Map.FromEntity(result.AsT0);
            await SendOkAsync(response, cancellationToken);

            return;
        }

        this.AddGeneralError("Authentication failed.");
        await SendErrorsAsync(cancellation: cancellationToken);
    }
}