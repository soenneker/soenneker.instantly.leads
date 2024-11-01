using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Instantly.Client.Registrars;
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
    public static void AddInstantlyLeadUtilAsSingleton(this IServiceCollection services)
    {
        services.AddInstantlyClientAsSingleton();
        services.TryAddSingleton<IInstantlyLeadUtil, InstantlyLeadUtil>();
    }

    /// <summary>
    /// Adds <see cref="IInstantlyLeadUtil"/> as a scoped service. <para/>
    /// </summary>
    public static void AddInstantlyLeadUtilAsScoped(this IServiceCollection services)
    {
        services.AddInstantlyClientAsSingleton();
        services.TryAddScoped<IInstantlyLeadUtil, InstantlyLeadUtil>();
    }
}
