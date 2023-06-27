using System.Diagnostics;

using FastEndApi.Data;
using FastEndApi.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace FastEndApi.Features.Comments.GetComments;

public interface IGetCommentsService
{
    Task<IReadOnlyList<Comment>> GetComments(CancellationToken cancellationToken);
}

public class GetCommentsService : IGetCommentsService
{
    private readonly IAppDbContextFactory _contextFactory;

    public GetCommentsService(IAppDbContextFactory contextFactory)
    {
        Debug.Assert(contextFactory is not null);

        _contextFactory = contextFactory;
    }

    public async Task<IReadOnlyList<Comment>> GetComments(CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContext(cancellationToken);

        var comments = await context.Comments
            .ToListAsync(cancellationToken);

        return comments;
    }
}