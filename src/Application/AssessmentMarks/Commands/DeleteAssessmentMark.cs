using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;

namespace CCAS.Application.AssessmentMarks.Commands;

public class DeleteAssessmentMarkCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteAssessmentMarkCommandHandler : IRequestHandler<DeleteAssessmentMarkCommand>
{
    private readonly IApplicationDbContext context;

    public DeleteAssessmentMarkCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Unit> Handle(DeleteAssessmentMarkCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.AssessmentMarks
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AssessmentMark), request.Id);
        }

        context.AssessmentMarks.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
