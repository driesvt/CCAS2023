using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Assessments.Commands;

public class UpdateAssessmentCommand : IRequest, IMapFrom<Assessment>
{
    public int Id { get; set; }
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

public class UpdateAssessmentCommandValidator : AbstractValidator<UpdateAssessmentCommand>
{
    private readonly IApplicationDbContext context;

    public UpdateAssessmentCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(AssessmentNameMustNotExist).WithMessage("A assessment with this name already exists");
        RuleFor(v => v.AssessmentCode).MustAsync(AssessmentCodeMustNotExist).WithMessage("A assessment with this code already exists");

        this.context = context;
    }

    async Task<bool> AssessmentNameMustNotExist(UpdateAssessmentCommand command, string? assessmentName, CancellationToken cancellationToken)
    {
        return await context.Assessments.CountAsync(p => p.Name == assessmentName && p.Id != command.Id) == 0 ? true : false;
    }
    async Task<bool> AssessmentCodeMustNotExist(UpdateAssessmentCommand command, string? assessmentCode, CancellationToken cancellationToken)
    {
        return await context.Assessments.CountAsync(p => p.AssessmentCode == assessmentCode && p.Id != command.Id) == 0 ? true : false;
    }
}

public class UpdateAssessmentCommandHandler : IRequestHandler<UpdateAssessmentCommand>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public UpdateAssessmentCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateAssessmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Assessments
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Assessment), request.Id);
        }

        entity = mapper.Map(request, entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

