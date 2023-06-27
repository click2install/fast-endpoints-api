
using FastEndApi.Extensions;

namespace FastEndApi.Features.Account.Login;

public class LoginDependencies : IFeatureDependencies
{
    public void Register(IServiceCollection services)
    {
        services.AddSingleton<ILoginService, LoginService>();
    }
}
