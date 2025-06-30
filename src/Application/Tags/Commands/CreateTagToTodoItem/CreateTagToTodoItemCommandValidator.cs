using FluentValidation;

namespace Todo_App.Application.Tags.Commands.CreateTagToTodoItem;
public class CreateTagToTodoItemCommandValidator : AbstractValidator<CreateTagToTodoItemCommand>
{
    public CreateTagToTodoItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag Name is required.")
            .MaximumLength(50).WithMessage("Tag Name must not exceed 50 characters.");
    }
}
