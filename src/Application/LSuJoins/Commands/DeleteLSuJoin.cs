using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;

namespace CCAS.Application.LSuJoins.Commands;

public class DeleteLSuJoinCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteLSuJoinCommandHandler : IRequestHandler<DeleteLSuJoinCommand>
{
    private readonly IApplicationDbContext context;

    public DeleteLSuJoinCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Unit> Handle(DeleteLSuJoinCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.LSuJoins
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(LSuJoin), request.Id);
        }

        context.LSuJoins.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
