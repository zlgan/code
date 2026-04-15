using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoChangeStream.DDD.Application.Common.Interfaces;
using MongoChangeStream.DDD.Infrastructure.ChangeStream;
using MongoChangeStream.DDD.Infrastructure.Persistence;

namespace MongoChangeStream.DDD.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var mongoSettings = configuration.GetSection(MongoSettings.SectionName).Get<MongoSettings>()
            ?? new MongoSettings();

        services.AddSingleton(mongoSettings);
        services.AddSingleton<MongoDbContext>();

        services.AddSingleton<IMongoChangeStreamProcessor, MongoChangeStreamProcessor>();
        services.AddHostedService<MongoChangeStreamProcessor>();

        return services;
    }

    public static IServiceCollection RegisterChangeStreamHandler<TEntity, THandler>(this IServiceCollection services, string collectionName)
        where THandler : class, IChangeEventHandler<TEntity>
    {
        services.AddScoped<IChangeEventHandler<TEntity>, THandler>();
        services.AddSingleton(sp =>
        {
            var processor = sp.GetRequiredService<IMongoChangeStreamProcessor>();
            var handlerType = typeof(IChangeEventHandler<TEntity>);
            var entityType = typeof(TEntity);

            var registrationMethod = typeof(IMongoChangeStreamProcessor)
                .GetMethod(nameof(IMongoChangeStreamProcessor.RegisterProcessor))?
                .MakeGenericMethod(entityType);

            if (registrationMethod != null)
            {
                var handlers = sp.GetServices(handlerType);
                registrationMethod.Invoke(processor, new object[] { collectionName, handlers });
            }

            return processor;
        });

        return services;
    }
}
