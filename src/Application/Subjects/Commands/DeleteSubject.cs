using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;

namespace CCAS.Application.Subjects.Commands;

public class DeleteSubjectCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand>
{
    private readonly IApplicationDbContext context;

    public DeleteSubjectCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Unit> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Subjects
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Subject), request.Id);
        }

        if (entity.Name == "ABC Trading")
        {
            throw new DeleteForbiddenException("Not allowed to delete ABC Trading");
        }

        context.Subjects.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
