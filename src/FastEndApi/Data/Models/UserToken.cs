namespace FastEndApi.Data.Models;

public class UserToken
{
    public Guid Id { get; private set; }

    public Guid UserId { get; set; }

    public string Value { get; set; } = default!;

    public DateTimeOffset Expiry { get; set; }
}
