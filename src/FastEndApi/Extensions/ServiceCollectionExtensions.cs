using System.Diagnostics;
using System.Reflection;

namespace FastEndApi.Extensions;

public interface IFeatureDependencies
{
    void Register(IServiceCollection services);
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFeatures(this IServiceCollection services, params Assembly[] assemblies)
    {
        var interfaceType = typeof(IFeatureDependencies);

        assemblies
          .SelectMany(a => a.GetTypes())
          .Where(type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type))
          .Select(type =>
          {
              var feature = Activator.CreateInstance(type) as IFeatureDependencies;
              Debug.Assert(feature is not null);

              return feature;
          })
          .ToList()
          .ForEach(feature => feature.Register(services));

        return services;
    }
}
