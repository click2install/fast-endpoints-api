using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using FastEndApi.Data;
using FastEndApi.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using OneOf;
using OneOf.Types;

namespace FastEndApi.Features.Account.Login;

public interface ILoginService
{
    Task<OneOf<User, None>> Login(string userName, string password, CancellationToken cancellationToken);
}

public class LoginService : ILoginService
{
    private readonly IAppDbContextFactory _contextFactory;
    private readonly IPasswordHasher<User> _hasher;

    public LoginService(IAppDbContextFactory contextFactory, IPasswordHasher<User> hasher)
    {
        Debug.Assert(contextFactory is not null);

        _contextFactory = contextFactory;
        _hasher = hasher;
    }

    public async Task<OneOf<User, None>> Login(string userName, string password, CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContext(cancellationToken);

        var user = await context.Users
            .AsTracking()
            .Include(x => x.Tokens
                .OrderByDescending(y => y.Expiry)
            )
            .SingleOrDefaultAsync(x => x.UserName == userName, cancellationToken);

        if (user is null)
        {
            return new None();
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result != PasswordVerificationResult.Success)
        {
            return new None();
        }

        if (user.Tokens.Count == 0 || user.Tokens.First().Expiry <= DateTimeOffset.UtcNow)
        {
            var token = await GenerateToken(cancellationToken);

            user.Tokens.Clear();
            user.Tokens.Add(new UserToken
            {
                Value = token,
                Expiry = DateTimeOffset.UtcNow.AddHours(1)
            });

            await context.SaveChangesAsync(cancellationToken);
        }

        return user;
    }

    private static async Task<string> GenerateToken(CancellationToken token)
    {
        var bytes = Encoding.UTF8.GetBytes($"{Guid.NewGuid()}-{DateTime.UtcNow:O}");

        await using var stream = new MemoryStream(bytes);
        var hash = await MD5.Create().ComputeHashAsync(stream, token);

        return Convert.ToBase64String(hash);
    }
}
