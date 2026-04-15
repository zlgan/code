namespace MongoChangeStream.DDD.Application.Common.Interfaces;

public interface IChangeEventHandler<T>
{
    string Name { get; }
    Task HandleInsertAsync(T entity, CancellationToken cancellationToken = default);
    Task HandleUpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task HandleDeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task HandleReplaceAsync(T entity, CancellationToken cancellationToken = default);
}
