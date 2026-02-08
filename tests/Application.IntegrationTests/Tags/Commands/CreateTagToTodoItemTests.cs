using NUnit.Framework;
using Todo_App.Application.Tags.Commands.CreateTagToTodoItem;
using Todo_App.Application.TodoItems.Commands.CreateTodoItem;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;
using Todo_App.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;

namespace Todo_App.Application.IntegrationTests.Tags.Commands;

using static Testing;
public class CreateTagToTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTagToTodoItemCommand();
        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateTagTodoItem()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var todoItemId = new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "Tasks"
        };

        var itemId = await SendAsync(todoItemId);

        var userId = await RunAsDefaultUserAsync();

        var command = new CreateTagToTodoItemCommand
        {
            TodoItemId = itemId,
            Name = "Test Tag"
        };

        var tagId = await SendAsync(command);

        var tag = await FindAsync<Tag>(tagId);

        tag.Should().NotBeNull();
        tag!.Name.Should().Be(command.Name);
        tag.Id.Should().Be(tagId);
        tag.CreatedBy.Should().Be(userId);
        tag.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        tag.LastModifiedBy.Should().Be(userId);
        tag.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}