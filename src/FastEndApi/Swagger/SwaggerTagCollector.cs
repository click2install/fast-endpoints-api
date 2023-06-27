using System.Diagnostics;
using System.Reflection;

namespace FastEndApi.Swagger;

public interface ISwaggerTags
{
    public IDictionary<string, string> GetTags();
}

public static class SwaggerTagCollector
{
    public static void Collect(Dictionary<string, string> dictionary, params Assembly[] assemblies)
    {
        var interfaceType = typeof(ISwaggerTags);

        assemblies
          .SelectMany(a => a.GetTypes())
          .Where(type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type))
          .SelectMany(type =>
          {
              var tag = Activator.CreateInstance(type) as ISwaggerTags;
              Debug.Assert(tag is not null);

              return tag.GetTags();
          })
          .ToList()
          .ForEach(tag => dictionary.Add(tag.Key, tag.Value));
    }
}
