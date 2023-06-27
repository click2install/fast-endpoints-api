using FastEndApi.Swagger;

namespace FastEndApi.Features.Comments;

public class SwaggerTags : ISwaggerTags
{
    public IDictionary<string, string> GetTags()
    {
        return new Dictionary<string, string>
        {
            { "Comments", "A user submitted comment." }
        };
    }
}