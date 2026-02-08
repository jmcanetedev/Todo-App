namespace Todo_App.Application.IntegrationTests.Tags.Commands;

using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Tags.Commands.CreateTagToTodoItem;
using Todo_App.Application.Tags.Commands.DeleteTagFromTodoItem;
using Todo_App.Application.Tags.Queries.GetTags;
using Todo_App.Application.TodoItems.Commands.CreateTodoItem;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;
using Todo_App.Domain.Entities;
using static Testing;
public class DeleteTagFromTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemIdAndTagId()
    {
        var command = new DeleteTagFromTodoItemCommand(99, 1);
        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }
    [Test]
    public async Task ShouldDeleteTagFromTodoItem()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });
        var todoItemId = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "Tasks"
        });
        var tagId = await SendAsync(new CreateTagToTodoItemCommand
        {
            TodoItemId = todoItemId,
            Name = "Test Tag"
        });

        var command = new DeleteTagFromTodoItemCommand(todoItemId, tagId);
        
        await SendAsync(command);

        var tag = await FindAsync<TodoItemTag>(tagId, todoItemId);

        tag.Should().BeNull();
    }
}
