using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.SSJoins.Commands;


public class CreateSSJoinCommand : IRequest<int>
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }

}

public class CreateSSJoinCommandValidator : AbstractValidator<CreateSSJoinCommand>
{
    private readonly IApplicationDbContext context;

    public CreateSSJoinCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.StudentId).NotEmpty();
        RuleFor(v => v.SubjectId).NotEmpty();
        this.context = context;
    }

}

public class CreateSSJoinCommandHandler : IRequestHandler<CreateSSJoinCommand, int>
{
    private readonly IApplicationDbContext context;

    public CreateSSJoinCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<int> Handle(CreateSSJoinCommand request, CancellationToken cancellationToken)
    {
        var entity = new SSJoin
        {
            StudentId = request.StudentId,
            SubjectId = request.SubjectId,
        };

        context.SSJoins.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


