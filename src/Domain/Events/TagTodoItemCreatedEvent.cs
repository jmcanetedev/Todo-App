namespace Todo_App.Domain.Events;
public class TagTodoItemCreatedEvent : BaseEvent
{
    public TagTodoItemCreatedEvent(Tag tag)
    {
        Item = tag;
    }
    public Tag Item { get; set; }
}
