namespace MongoChangeStream.DDD.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
