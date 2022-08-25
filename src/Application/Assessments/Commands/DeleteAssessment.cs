using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;

namespace CCAS.Application.Assessments.Commands;

public class DeleteAssessmentCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteAssessmentCommandHandler : IRequestHandler<DeleteAssessmentCommand>
{
    private readonly IApplicationDbContext context;

    public DeleteAssessmentCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Unit> Handle(DeleteAssessmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Assessments
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Assessment), request.Id);
        }

        if (entity.Name == "ABC Trading")
        {
            throw new DeleteForbiddenException("Not allowed to delete ABC Trading");
        }

        context.Assessments.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
