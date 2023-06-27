
using FastEndApi.Extensions;

namespace FastEndApi.Features.Authorization.ApiKey;

public class ApiKeyDependencies : IFeatureDependencies
{
    public void Register(IServiceCollection services)
    {
        services.AddSingleton<IApiKeyService, ApiKeyService>();
    }
}
