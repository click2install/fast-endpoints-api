namespace FastEndApi.Data.Models;

public class User
{
    public Guid Id { get; private set; }

    public string UserName { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public DateTimeOffset Created { get; set; }

    public ICollection<UserToken> Tokens { get; private set; } = new HashSet<UserToken>();

    public ICollection<Comment> Comments { get; private set; } = new HashSet<Comment>();
}
