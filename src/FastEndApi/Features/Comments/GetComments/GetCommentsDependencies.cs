
using FastEndApi.Extensions;

namespace FastEndApi.Features.Comments.GetComments;

public class GetCommentsDependencies : IFeatureDependencies
{
    public void Register(IServiceCollection services)
    {
        services.AddSingleton<IGetCommentsService, GetCommentsService>();
    }
}
