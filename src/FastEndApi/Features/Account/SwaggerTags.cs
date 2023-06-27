using FastEndApi.Swagger;

namespace FastEndApi.Features.Account;

public class SwaggerTags : ISwaggerTags
{
    public IDictionary<string, string> GetTags()
    {
        return new Dictionary<string, string>
        {
            { "Account", "A users account." }
        };
    }
}