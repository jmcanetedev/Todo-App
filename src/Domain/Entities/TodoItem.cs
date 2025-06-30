namespace Todo_App.Domain.Entities;

public class TodoItem : BaseAuditableEntity
{
    public int ListId { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }

    public PriorityLevel Priority { get; set; }

    public DateTime? Reminder { get; set; }
    public Colour Colour { get; private set; } = Colour.White;

    private readonly List<TodoItemTag> _tags = new();

    public IReadOnlyCollection<TodoItemTag> Tags => _tags.AsReadOnly();

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value == true && _done == false)
            {
                AddDomainEvent(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }

    public TodoList List { get; set; } = null!;

    public void SetBackgroundColour(string code)
    {
        Colour = Colour.From(code);
    }

    public void AddTag(Tag tag)
    {
        tag.AddDomainEvent(new TagTodoItemCreatedEvent(tag));

        if (_tags.All(t => t.Tag.Name != tag.Name))
            _tags.Add(new TodoItemTag { Tag = tag, TodoItemId = Id });
    }
    public void RemoveTag(int tagId)
    {
        _tags.RemoveAll(t=>t.TagId == tagId && t.TodoItemId == Id);
    }
}
