using MongoChangeStream.DDD.Application.Common.Interfaces;
using MongoChangeStream.DDD.Domain.Entities;

namespace MongoChangeStream.DDD.Application.Handlers;

public class ProductChangeStreamHandler : IChangeEventHandler<Product>
{
    public ProductChangeStreamHandler()
    {
    }

    public string Name => "ProductChangeStreamHandler";

    public Task HandleInsertAsync(Product entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Product inserted: {entity.Id} - {entity.Name} - {entity.Price}");
        return Task.CompletedTask;
    }

    public Task HandleUpdateAsync(Product entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Product updated: {entity.Id} - {entity.Name} - {entity.Price}");
        return Task.CompletedTask;
    }

    public Task HandleDeleteAsync(Product entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Product deleted: {entity.Id}");
        return Task.CompletedTask;
    }

    public Task HandleReplaceAsync(Product entity, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Product replaced: {entity.Id} - {entity.Name} - {entity.Price}");
        return Task.CompletedTask;
    }
}
