using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.AssessmentMarks.Commands;


public class CreateAssessmentMarkCommand : IRequest<int>
{
    public int AssessmentId { get; set; }
    public int StudentId { get; set; }
    public int? Mark { get; set; }
}

public class CreateAssessmentMarkCommandValidator : AbstractValidator<CreateAssessmentMarkCommand>
{
    private readonly IApplicationDbContext context;

    public CreateAssessmentMarkCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.AssessmentId).NotEmpty();
        RuleFor(v => v.StudentId).NotEmpty();
        this.context = context;
    }

}

public class CreateAssessmentMarkCommandHandler : IRequestHandler<CreateAssessmentMarkCommand, int>
{
    private readonly IApplicationDbContext context;

    public CreateAssessmentMarkCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<int> Handle(CreateAssessmentMarkCommand request, CancellationToken cancellationToken)
    {
        var entity = new AssessmentMark
        {
            AssessmentId = request.AssessmentId,
            StudentId = request.StudentId,
            Mark = request.Mark
        };

        context.AssessmentMarks.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


