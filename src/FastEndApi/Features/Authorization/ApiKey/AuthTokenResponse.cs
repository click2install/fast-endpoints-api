namespace FastEndApi.Features.Authorization.ApiKey;

public readonly record struct AuthTokenResponse
{
    public Guid UserId { get; init; }


    public bool TokenExpired { get; init; }
}
