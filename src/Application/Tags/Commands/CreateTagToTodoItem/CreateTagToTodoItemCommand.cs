using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.Tags.Queries.GetTags;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Events;
using Todo_App.Domain.Exceptions;

namespace Todo_App.Application.Tags.Commands.CreateTagToTodoItem;

public record CreateTagToTodoItemCommand : IRequest<int>
{
    public int TodoItemId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateTagTodoItemCommandHandler : IRequestHandler<CreateTagToTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateTagTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateTagToTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todo = await _context.TodoItems
            .Include(t => t.Tags)
            .ThenInclude(todoTag => todoTag.Tag)
            .FirstOrDefaultAsync(t => t.Id == request.TodoItemId, cancellationToken)
     ?? throw new NotFoundException(nameof(TodoItem), request.TodoItemId);

        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == request.Name, cancellationToken)
            ?? new Tag(request.Name);

        var todoTag = await _context.TodoItemTags
            .ProjectTo<TodoItemTagDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c=>c.Tag.Name == request.Name && c.TodoItemId == request.TodoItemId, cancellationToken);

        if (todoTag is not null)
            throw new DuplicateTagException(request.Name);

        if (tag.Id == 0)
            _context.Tags.Add(tag);

        todo.AddTag(tag);

        await _context.SaveChangesAsync(cancellationToken);

        return tag.Id;

    }
}