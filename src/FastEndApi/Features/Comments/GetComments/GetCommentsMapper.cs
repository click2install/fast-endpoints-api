using FastEndApi.Data.Models;

namespace FastEndApi.Features.Comments.GetComments;

public class GetCommentsMapper : ResponseMapper<GetCommentsResponse, IReadOnlyList<Comment>>
{
    public override GetCommentsResponse FromEntity(IReadOnlyList<Comment> comments)
    {
        return new GetCommentsResponse
        {
            Comments = comments
                .Select(x => new GetCommentsComment
                {
                    Created = x.Created,
                    Title = x.Title,
                    Content = x.Content
                })
                .ToList()
        };
    }
}
