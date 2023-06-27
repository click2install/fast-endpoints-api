namespace FastEndApi.Features.Account.Login;

public readonly record struct LoginResponse
{
    public string ApiToken { get; init; }

    public string UserName { get; init; }
}
