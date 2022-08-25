using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.LSuJoins.Commands;


public class CreateLSuJoinCommand : IRequest<int>
{
    public int LecturerId { get; set; }
    public int SubjectId { get; set; }

}

public class CreateLSuJoinCommandValidator : AbstractValidator<CreateLSuJoinCommand>
{
    private readonly IApplicationDbContext context;

    public CreateLSuJoinCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.LecturerId).NotEmpty();
        RuleFor(v => v.SubjectId).NotEmpty();
        this.context = context;
    }

}

public class CreateLSuJoinCommandHandler : IRequestHandler<CreateLSuJoinCommand, int>
{
    private readonly IApplicationDbContext context;

    public CreateLSuJoinCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<int> Handle(CreateLSuJoinCommand request, CancellationToken cancellationToken)
    {
        var entity = new LSuJoin
        {
            LecturerId = request.LecturerId,
            SubjectId = request.SubjectId,
        };

        context.LSuJoins.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


