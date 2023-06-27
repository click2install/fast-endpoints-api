
using System.Diagnostics;

namespace FastEndApi.Features.Comments.GetComments;

public class GetCommentsEndpoint : EndpointWithoutRequest<GetCommentsResponse, GetCommentsMapper>
{
    private readonly IGetCommentsService _service;

    public GetCommentsEndpoint(IGetCommentsService service)
    {
        Debug.Assert(service is not null);

        _service = service;
    }

    public override void Configure()
    {
        Get("/comments");

        Summary(s => s.Summary = "Gets a list of comments");
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var comments = await _service.GetComments(cancellationToken);
        var response = Map.FromEntity(comments);

        await SendOkAsync(response, cancellationToken);
    }
}