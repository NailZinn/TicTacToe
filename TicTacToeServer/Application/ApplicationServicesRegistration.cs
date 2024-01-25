using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServicesRegistration
{
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly));

        return serviceCollection;
    }
}
