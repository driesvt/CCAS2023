using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Assessments.Commands;


public class CreateAssessmentCommand : IRequest<int>
{
    public string? Name { get; set; }
    public string? AssessmentCode { get; set; }
    public string? Author { get; set; }
    public string? Moderator { get; set; }
    public string? Details { get; set; }
    public int? MaxMark { get; set; }
    public string? Weighting { get; set; }
    public DateTime ModerationSubmitDate { get; set; }
    public DateTime ModerationCompleteDate { get; set; }
    public DateTime DueDate { get; set; }
    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }
}

public class CreateAssessmentCommandValidator : AbstractValidator<CreateAssessmentCommand>
{
    private readonly IApplicationDbContext context;

    public CreateAssessmentCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(AssessmentNameMustNotExist).WithMessage("A assessment with this name already exists");
        RuleFor(v => v.AssessmentCode).MustAsync(AssessmentCodeMustNotExist).WithMessage("A assessment with this code already exists");
        this.context = context;
    }

    async Task <bool> AssessmentNameMustNotExist(string? assessmentName, CancellationToken cancellationToken)
    {
        return await context.Assessments.CountAsync(p => p.Name == assessmentName) == 0 ? true : false;
    }
    async Task<bool> AssessmentCodeMustNotExist(string? assessmentCode, CancellationToken cancellationToken)
    {
        return await context.Assessments.CountAsync(p => p.Name == assessmentCode) == 0 ? true : false;
    }
}

public class CreateAssessmentCommandHandler : IRequestHandler<CreateAssessmentCommand, int>
{
    private readonly IApplicationDbContext context;

    public CreateAssessmentCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<int> Handle(CreateAssessmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Assessment
        {
            Name = request.Name,
            AssessmentCode = request.AssessmentCode,
            Author = request.Author,
            Moderator = request.Moderator,
            Details = request.Details,
            MaxMark = request.MaxMark,
            Weighting = request.Weighting,
            ModerationSubmitDate = request.ModerationSubmitDate,
            ModerationCompleteDate = request.ModerationCompleteDate,
            DueDate = request.DueDate,
            SubjectId = request.SubjectId,
            Subject = request.Subject,
        };

        context.Assessments.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


