using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Oluso.Data.AutoMapping;

/// <summary>
/// auto load mapping specifications. mapping specification are created in the Dtos
/// where you specify which entity maps the marked class. Dtos must inherit from
/// <see cref="IMapFrom{T}"/> interface
/// </summary>
public class AutoMapping : AutoMapper.Profile
{
    private readonly ILogger<AutoMapping>? _logger;

    private readonly List<string> _avoidPrefixes = new()
    {
        "Microsoft", "System", "AutoMapper", "MediatR", "Swashbuckle", "HotChocolate", 
        "Scrutor", "Agile", "JetBrains", "netstandard", "GreenDonut",  "Oluso"
    };

    /// <summary>
    /// returns a new <see cref="AutoMapping"/> instance
    /// </summary>
    public AutoMapping()
    {
        var assemblies =
            (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            let found = _avoidPrefixes.Any(x => assembly.FullName != null &&
                                                assembly.FullName.StartsWith(x))
            where(!found)
            select assembly).ToList();

        foreach (var assembly in assemblies)
        {
            LoadMapping(assembly);
        }
    }
    
    /// <summary>
    /// returns a new <see cref="AutoMapping"/> instance
    /// </summary>
    /// <param name="logger"></param>
    public AutoMapping(ILogger<AutoMapping> logger)
    {
        _logger = logger;
        _logger?.LogDebug("Loading entities AutoMapping");

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            LoadMapping(assembly);
        }
        
        _logger?.LogDebug("Available mappings loaded");
    }

    private void LoadMapping(Assembly assembly)
    {
        var mapFrom = typeof(IMapFrom<>);
        var mappingMethod = nameof(IMapFrom<object>.Mapping);
        bool HasInterface(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == mapFrom;
        var types = assembly.GetExportedTypes().Where(x => x.GetInterfaces().Any(HasInterface) && 
                                                           x.Name.Contains("Oluso.Data.AutoMapping")).ToList();
        var argTypes = new Type[] { typeof(AutoMapper.Profile) };

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod(mappingMethod);
            if (methodInfo != null)
                methodInfo?.Invoke(instance, new object[] { this });
            else
            {
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();
                if (interfaces.Count <= 0) continue;

                foreach (var interfaceMethod in interfaces
                             .Select(x => x.GetMethod(mappingMethod, argTypes)))
                {
                    interfaceMethod?.Invoke(instance, new object[] { this });
                }
            }
        }
    }
}