using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.Tags.Commands.DeleteTagFromTodoItem;

public record DeleteTagFromTodoItemCommand(int TodoItemId, int TagId) : IRequest;

public class DeleteTagFromTodoItemCommandHandler : IRequestHandler<DeleteTagFromTodoItemCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteTagFromTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteTagFromTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todo = await _context.TodoItems
            .Include(t => t.Tags).ThenInclude(tt => tt.Tag)
            .FirstOrDefaultAsync(t => t.Id == request.TodoItemId, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoItem), request.TodoItemId);

        todo.RemoveTag(request.TagId);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;

    }
}