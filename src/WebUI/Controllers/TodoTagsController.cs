using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.Tags.Commands.CreateTagToTodoItem;
using Todo_App.Application.Tags.Commands.DeleteTagFromTodoItem;
using Todo_App.Application.Tags.Queries.GetTags;

namespace Todo_App.WebUI.Controllers;

public class TodoTagsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TagVm>> Get()
    {
        return await Mediator.Send(new GetTagsQuery());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTagToTodoItemCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}/{todoItemId}")]
    public async Task<ActionResult> Delete(int id, int todoItemId)
    {
        await Mediator.Send(new DeleteTagFromTodoItemCommand(id, todoItemId));

        return NoContent();
    }
}
