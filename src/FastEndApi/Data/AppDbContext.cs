using System.Data.Common;
using System.Globalization;
using System.Linq.Expressions;

using EFCore.NamingConventions.Internal;

using FastEndApi.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using Npgsql;

namespace FastEndApi.Data;

public interface IAppDbContext : IAsyncDisposable
{
    DbSet<Comment> Comments { get; }

    DbSet<User> Users { get; }

    DbSet<UserToken> UserTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken token = default);
}

public class AppDbContext : DbContext, IAppDbContext
{
    private static INameRewriter Rewriter => new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

    private static string SqlName<T>(Expression<Func<T, string>> expression)
    {
        static MemberExpression ExtractMemberExpression(Expression expression)
        {
            return expression is MemberExpression memberExpression
                ? memberExpression
                : expression is UnaryExpression unaryExpression
                    ? ExtractMemberExpression(unaryExpression.Operand)
                    : throw new ArgumentException($"Invalid expression type. Expected {nameof(MemberExpression)} or {nameof(UnaryExpression)}.");
        }

        var memberExpression = ExtractMemberExpression(expression.Body);
        var value = memberExpression.Member.Name;

        return Rewriter.RewriteName(value);
    }

    public DbSet<Comment> Comments { get; private set; } = default!;

    public DbSet<User> Users { get; private set; } = default!;

    public DbSet<UserToken> UserTokens { get; private set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.UseCollation("en_US.utf8");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Remove(typeof(TableNameFromDbSetConvention));
    }

    public static DbDataSource CreateDataSource(string connectionString)
    {
        var builder = new NpgsqlDataSourceBuilder(connectionString);

        // map enums, database objects

        return builder.Build();
    }
}
