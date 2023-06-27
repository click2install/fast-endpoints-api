
using FastEndApi.Extensions;

namespace FastEndApi.Features.Account.Register;

public class RegisterDependencies : IFeatureDependencies
{
    public void Register(IServiceCollection services)
    {
        services.AddSingleton<IRegisterService, RegisterService>();
    }
}
