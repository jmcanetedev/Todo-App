using Todo_App.Application.Common.Mappings;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tags.Queries.GetTags;
public class TodoItemTagDto : IMapFrom<TodoItemTag>
{
    public int TodoItemId { get; set; }
    public int TagId { get; set; }
    public TagDto? Tag { get; set; } = null;
}
