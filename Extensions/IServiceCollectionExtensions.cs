using Microsoft.Extensions.DependencyInjection;
using OpenSky.Core.Configuration;
using OpenSky.Core.Option;
using OpenSky.Core.Service;

namespace OpenSky.AspNetCore.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> to add OSS client services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the OSS client services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="config">The OSS configuration settings.</param>
    /// <param name="option">Optional OSS client options.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddOssClient(this IServiceCollection services, OssConfiguration config, OssOption? option = null) =>
        services.AddOsClientImpl(config, option);

    /// <summary>
    /// Adds the OSS client services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="config">An action to configure the OSS settings.</param>
    /// <param name="option">An optional action to configure the OSS client options.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddOssClient(this IServiceCollection services, Action<OssConfiguration> config, Action<OssOption>? option = null)
    {
        var configuration = new OssConfiguration();
        config(configuration);

        var ossOption = new OssOption();
        option?.Invoke(ossOption);

        return services.AddOsClientImpl(configuration, option == null ? null : ossOption);
    }

    /// <summary>
    /// Adds the OSS client services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="config">The OSS configuration settings.</param>
    /// <param name="option">Optional OSS client options.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddOsClientImpl(this IServiceCollection services, OssConfiguration config, OssOption? option = null)
    {
        services.AddHttpClient<IOpenSkyService, OpenSkyService>(e =>
        {
            e.BaseAddress = new Uri($"{(config.UseHttps ? "https" : "http")}://{config.BaseUrl}/{config.Version}");

            if (option != null)
            {
                e.Timeout = option.Timeout;
            }
        });

        services.AddTransient<OssConfiguration>(_ => config);

        if (option != null)
        {
            services.AddTransient<OssOption>(_ => option);
        }

        return services;
    }
}