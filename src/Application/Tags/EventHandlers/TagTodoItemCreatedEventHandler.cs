using MediatR;
using Microsoft.Extensions.Logging;
using Todo_App.Application.TodoItems.EventHandlers;
using Todo_App.Domain.Events;

namespace Todo_App.Application.Tags.EventHandlers;
public class TagTodoItemCreatedEventHandler : INotificationHandler<TagTodoItemCreatedEvent>
{
    private readonly ILogger<TagTodoItemCreatedEventHandler> _logger;

    public TagTodoItemCreatedEventHandler(ILogger<TagTodoItemCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TagTodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Todo_App Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
