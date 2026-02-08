using FluentValidation;

namespace Todo_App.Application.Tags.Commands.CreateTagToTodoItem;
public class CreateTagToTodoItemCommandValidator : AbstractValidator<CreateTagToTodoItemCommand>
{
    public CreateTagToTodoItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .NotEmpty();

        RuleFor(x => x.TodoItemId).Must(x => x > 0)
            .WithMessage("TodoItemId must be greater than 0")
            .NotEmpty();
    }
}
