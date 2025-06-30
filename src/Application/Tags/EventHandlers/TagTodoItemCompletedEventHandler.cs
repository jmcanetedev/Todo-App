using MediatR;
using Microsoft.Extensions.Logging;
using Todo_App.Domain.Events;

namespace Todo_App.Application.Tags.EventHandlers;
public class TagTodoItemCompletedEventHandler : INotificationHandler<TagTodoItemCompletedEvent>
{
    private readonly ILogger<TagTodoItemCompletedEventHandler> _logger;

    public TagTodoItemCompletedEventHandler(ILogger<TagTodoItemCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TagTodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Todo_App Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
