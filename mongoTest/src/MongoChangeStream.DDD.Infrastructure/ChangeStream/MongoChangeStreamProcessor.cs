using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoChangeStream.DDD.Application.Common.Interfaces;
using MongoChangeStream.DDD.Infrastructure.Persistence;

namespace MongoChangeStream.DDD.Infrastructure.ChangeStream;

public class MongoChangeStreamProcessor : IMongoChangeStreamProcessor, IHostedService
{
    private readonly MongoDbContext _context;
    private readonly ILogger<MongoChangeStreamProcessor> _logger;
    private readonly Dictionary<string, Type> _collectionHandlers = new();
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly CancellationTokenSource _shutdownCts = new();

    private Task? _processingTask;
    private bool _isDisposed;

    public MongoChangeStreamProcessor(
        MongoDbContext context,
        ILogger<MongoChangeStreamProcessor> logger,
        IServiceScopeFactory scopeFactory)
    {
        _context = context;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public void RegisterProcessor<T>(string collectionName, IChangeEventHandler<T> handler)
    {
        RegisterProcessor(collectionName, new[] { handler });
    }

    public void RegisterProcessor<T>(string collectionName, IEnumerable<IChangeEventHandler<T>> handlers)
    {
        if (handlers.Any())
        {
            _collectionHandlers[collectionName] = typeof(T);
            _logger.LogInformation(
                "Registered {HandlerCount} change handlers for collection '{CollectionName}' of type {EntityType}",
                handlers.Count(), collectionName, typeof(T).Name);
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting MongoDB Change Stream Processor...");

        if (_processingTask != null)
        {
            _logger.LogWarning("Change Stream Processor is already running");
            return;
        }

        _processingTask = ProcessChangesAsync(_shutdownCts.Token);
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping MongoDB Change Stream Processor...");

        if (_processingTask == null)
        {
            _logger.LogWarning("Change Stream Processor is not running");
            return;
        }

        _shutdownCts.Cancel();

        try
        {
            await _processingTask;
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            _processingTask = null;
            _logger.LogInformation("MongoDB Change Stream Processor stopped");
        }
    }

    private async Task ProcessChangesAsync(CancellationToken cancellationToken)
    {
        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
        };

        var collectionNames = _collectionHandlers.Keys.ToList();
        var database = _context.Database;

        _logger.LogInformation("Watching {CollectionCount} collections: {Collections}",
            collectionNames.Count, string.Join(", ", collectionNames));

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var changeStream = database.Watch(options, cancellationToken);

                await foreach (var change in changeStream.ToAsyncEnumerable())
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    await ProcessChangeAsync(change, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in change stream. Retrying in 5 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }

    private async Task ProcessChangeAsync(ChangeStreamDocument<BsonDocument> change, CancellationToken cancellationToken)
    {
        try
        {
            var collectionName = change.CollectionNamespace.CollectionName;
            if (string.IsNullOrEmpty(collectionName) || !_collectionHandlers.TryGetValue(collectionName, out var entityType))
            {
                return;
            }

            var operationType = change.OperationType;
            var fullDocument = change.FullDocument;

            if (fullDocument == null && operationType != ChangeStreamOperationType.Delete)
            {
                _logger.LogWarning("Received change with null full document for collection '{CollectionName}'", collectionName);
                return;
            }

            _logger.LogInformation("Processing {OperationType} on collection '{CollectionName}'",
                operationType, collectionName);

            using var scope = _scopeFactory.CreateScope();
            var handlerType = typeof(IChangeEventHandler<>).MakeGenericType(entityType);
            var handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                try
                {
                    var method = operationType switch
                    {
                        ChangeStreamOperationType.Insert => nameof(IChangeEventHandler<object>.HandleInsertAsync),
                        ChangeStreamOperationType.Update => nameof(IChangeEventHandler<object>.HandleUpdateAsync),
                        ChangeStreamOperationType.Delete => nameof(IChangeEventHandler<object>.HandleDeleteAsync),
                        ChangeStreamOperationType.Replace => nameof(IChangeEventHandler<object>.HandleReplaceAsync),
                        _ => null
                    };

                    if (method != null)
                    {
                        var handleMethod = handler?.GetType().GetMethod(method);
                        if (handleMethod != null)
                        {
                            var documentToUse = operationType == ChangeStreamOperationType.Delete
                                ? change.DocumentKey
                                : fullDocument;

                            if (documentToUse != null)
                            {
                                var entity = BsonSerializer.Deserialize(documentToUse, entityType);
                                await (Task)handleMethod.Invoke(handler!, new[] { entity, cancellationToken })!;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing change with handler '{HandlerName}'",
                        handler?.GetType().Name);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing change stream document");
        }
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _shutdownCts.Cancel();
        _shutdownCts.Dispose();
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }
}
