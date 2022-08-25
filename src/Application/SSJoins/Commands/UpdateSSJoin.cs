using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.SSJoins.Commands;

public class UpdateSSJoinCommand : IRequest, IMapFrom<SSJoin>
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }

}

public class UpdateSSJoinCommandValidator : AbstractValidator<UpdateSSJoinCommand>
{
    private readonly IApplicationDbContext context;

    public UpdateSSJoinCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(v => v.StudentId).NotEmpty();
        RuleFor(v => v.SubjectId).NotEmpty();

        this.context = context;
    }

}

public class UpdateSSJoinCommandHandler : IRequestHandler<UpdateSSJoinCommand>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public UpdateSSJoinCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateSSJoinCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.SSJoins
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(SSJoin), request.Id);
        }

        entity = mapper.Map(request, entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

