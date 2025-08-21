using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Instantly.ClientUtil.Registrars;
using Soenneker.Instantly.Leads.Abstract;

namespace Soenneker.Instantly.Leads.Registrars;

/// <summary>
/// A .NET typesafe implementation of Instantly's Lead API
/// </summary>
public static class InstantlyLeadUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IInstantlyLeadUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddInstantlyLeadUtilAsSingleton(this IServiceCollection services)
    {
        services.AddInstantlyOpenApiClientUtilAsSingleton().TryAddSingleton<IInstantlyLeadUtil, InstantlyLeadUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IInstantlyLeadUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddInstantlyLeadUtilAsScoped(this IServiceCollection services)
    {
        services.AddInstantlyOpenApiClientUtilAsSingleton().TryAddScoped<IInstantlyLeadUtil, InstantlyLeadUtil>();

        return services;
    }
}