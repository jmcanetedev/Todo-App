using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;

namespace Todo_App.Application.Tags.Queries.GetTags;

public record GetTagsQuery(int topLimit = 10): IRequest<TagVm>;

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, TagVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetTagsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TagVm> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        return new TagVm
        {
            Tags = await _context.Tags.AsNoTracking().ProjectTo<TagDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken),

            List = await _context.TodoItemTags
            .GroupBy(x => x.Tag.Name)
            .OrderByDescending(g => g.Count())
            .Take(request.topLimit)
            .Select(g => g.Key)
            .ToListAsync(cancellationToken)
        };
    }
}