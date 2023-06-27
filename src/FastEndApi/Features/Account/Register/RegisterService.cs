using System.Diagnostics;

using FastEndApi.Data;
using FastEndApi.Data.Models;

using Microsoft.AspNetCore.Identity;

using OneOf;
using OneOf.Types;

namespace FastEndApi.Features.Account.Register;

public interface IRegisterService
{
    Task<OneOf<User, None>> Register(string userName, string password, CancellationToken cancellationToken);
}

public class RegisterService : IRegisterService
{
    private readonly IAppDbContextFactory _contextFactory;
    private readonly IPasswordHasher<User> _hasher;

    public RegisterService(IAppDbContextFactory contextFactory, IPasswordHasher<User> hasher)
    {
        Debug.Assert(contextFactory is not null);
        Debug.Assert(hasher is not null);

        _contextFactory = contextFactory;
        _hasher = hasher;
    }

    public async Task<OneOf<User, None>> Register(string userName, string password, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContext(cancellationToken);

        try
        {
            var user = new User
            {
                Created = DateTimeOffset.UtcNow,
                UserName = userName
            };
            user.PasswordHash = _hasher.HashPassword(user, password);

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }
        catch
        {
            return new None();
        }
    }
}
