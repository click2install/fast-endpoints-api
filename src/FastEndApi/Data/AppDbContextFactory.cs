using System.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace FastEndApi.Data;

public interface IAppDbContextFactory
{
    Task<IAppDbContext> CreateDbContext(CancellationToken cancellationToken);
}

public class AppDbContextFactory : IAppDbContextFactory
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public AppDbContextFactory(IDbContextFactory<AppDbContext> factory)
    {
        Debug.Assert(factory is not null);

        _factory = factory;
    }

    public async Task<IAppDbContext> CreateDbContext(CancellationToken cancellationToken)
    {
        var context = await _factory.CreateDbContextAsync(cancellationToken);

        return context;
    }
}
