using MongoChangeStream.DDD.Application.Common.Interfaces;

namespace MongoChangeStream.DDD.Infrastructure.ChangeStream;

public interface IMongoChangeStreamProcessor
{
    void RegisterProcessor<T>(string collectionName, IChangeEventHandler<T> handler);
    void RegisterProcessor<T>(string collectionName, IEnumerable<IChangeEventHandler<T>> handlers);
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}
