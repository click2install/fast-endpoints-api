using FastEndApi.Extensions;

namespace FastEndApi.Features.Account.Register;

public class RegisterEndpoint : Endpoint<RegisterRequest>
{
    private readonly IRegisterService _service;

    public RegisterEndpoint(IRegisterService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Post("/account/register");

        Summary(s =>
        {
            s.Summary = "Registers a new user account.";
            s.Description = "Once registered the user is required to login to obtain an API key to use in all subsequent requests.";
        });
    }

    public override async Task HandleAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.Register(request.UserName, request.Password, cancellationToken);
        if (result.IsT0)
        {
            await SendNoContentAsync(cancellationToken);
            return;
        }

        this.AddGeneralError("Registration failed.");
        await SendErrorsAsync(cancellation: cancellationToken);
    }
}