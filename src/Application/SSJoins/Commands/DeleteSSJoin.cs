using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;

namespace CCAS.Application.SSJoins.Commands;

public class DeleteSSJoinCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteSSJoinCommandHandler : IRequestHandler<DeleteSSJoinCommand>
{
    private readonly IApplicationDbContext context;

    public DeleteSSJoinCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Unit> Handle(DeleteSSJoinCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.SSJoins
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(SSJoin), request.Id);
        }

        context.SSJoins.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
