
using System.Diagnostics;

using FastEndApi.Data;

using Microsoft.EntityFrameworkCore;

using OneOf;
using OneOf.Types;

namespace FastEndApi.Features.Authorization.ApiKey;

public interface IApiKeyService
{
    Task<OneOf<AuthTokenResponse, None>> ValidateAuthToken(string token, CancellationToken cancellationToken);
}

public class ApiKeyService : IApiKeyService
{
    private readonly IAppDbContextFactory _contextFactory;

    public ApiKeyService(IAppDbContextFactory contextFactory)
    {
        Debug.Assert(contextFactory is not null);

        _contextFactory = contextFactory;
    }

    public async Task<OneOf<AuthTokenResponse, None>> ValidateAuthToken(string token, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContext(cancellationToken);

        var user = await context.Users
            .Include(x => x.Tokens
                .OrderByDescending(t => t.Expiry)
                .Where(t => t.Value == token)
            )
            .SingleOrDefaultAsync(cancellationToken);

        return user is null
            ? new None()
            : new AuthTokenResponse
            {
                UserId = user.Id,
                TokenExpired = user.Tokens.First().Expiry <= DateTimeOffset.UtcNow
            };
    }
}