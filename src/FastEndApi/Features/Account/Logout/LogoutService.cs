using System.Diagnostics;

using FastEndApi.Data;

using Microsoft.EntityFrameworkCore;

using OneOf;
using OneOf.Types;

namespace FastEndApi.Features.Account.Logout;

public interface ILogoutService
{
    Task<OneOf<Success, None>> Logout(Guid userId, CancellationToken cancellationToken);
}

public class LogoutService : ILogoutService
{
    private readonly IAppDbContextFactory _contextFactory;

    public LogoutService(IAppDbContextFactory contextFactory)
    {
        Debug.Assert(contextFactory is not null);

        _contextFactory = contextFactory;
    }

    public async Task<OneOf<Success, None>> Logout(Guid userId, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContext(cancellationToken);

        var user = await context.Users
            .AsTracking()
            .Include(x => x.Tokens)
            .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
        {
            return new None();
        }

        user.Tokens.Clear();
        await context.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}
