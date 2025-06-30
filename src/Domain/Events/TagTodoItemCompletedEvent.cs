namespace Todo_App.Domain.Events;
public class TagTodoItemCompletedEvent : BaseEvent
{
    public TagTodoItemCompletedEvent(Tag tag)
    {
        Item = tag;
    }
    public Tag Item { get; set; }
}
