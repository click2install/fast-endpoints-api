
using FastEndApi.Extensions;

namespace FastEndApi.Features.Account.Logout;

public class LogoutDependencies : IFeatureDependencies
{
    public void Register(IServiceCollection services)
    {
        services.AddSingleton<ILogoutService, LogoutService>();
    }
}
