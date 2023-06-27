namespace FastEndApi.Data.Models;

public class Comment
{
    public Guid Id { get; private set; }

    public Guid UserId { get; set; }

    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;

    public DateTimeOffset Created { get; set; }
}
