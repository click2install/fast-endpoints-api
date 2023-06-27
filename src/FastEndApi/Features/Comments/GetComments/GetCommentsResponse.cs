namespace FastEndApi.Features.Comments.GetComments;

public readonly record struct GetCommentsResponse
{
    public IReadOnlyList<GetCommentsComment> Comments { get; init; }
}

public readonly record struct GetCommentsComment
{
    public string Title { get; init; }

    public string Content { get; init; }

    public DateTimeOffset Created { get; init; }
}
