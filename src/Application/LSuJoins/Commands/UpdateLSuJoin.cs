using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.LSuJoins.Commands;

public class UpdateLSuJoinCommand : IRequest, IMapFrom<LSuJoin>
{
    public int Id { get; set; }
    public int LecturerId { get; set; }
    public int SubjectId { get; set; }

}

public class UpdateLSuJoinCommandValidator : AbstractValidator<UpdateLSuJoinCommand>
{
    private readonly IApplicationDbContext context;

    public UpdateLSuJoinCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(v => v.LecturerId).NotEmpty();
        RuleFor(v => v.SubjectId).NotEmpty();

        this.context = context;
    }

}

public class UpdateLSuJoinCommandHandler : IRequestHandler<UpdateLSuJoinCommand>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public UpdateLSuJoinCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateLSuJoinCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.LSuJoins
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(LSuJoin), request.Id);
        }

        entity = mapper.Map(request, entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

