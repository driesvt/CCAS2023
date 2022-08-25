using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.AssessmentMarks.Commands;

public class UpdateAssessmentMarkCommand : IRequest, IMapFrom<AssessmentMark>
{
    public int Id { get; set; }
    public int AssessmentId { get; set; }
    public int StudentId { get; set; }
    public int? Mark { get; set; }
}

public class UpdateAssessmentMarkCommandValidator : AbstractValidator<UpdateAssessmentMarkCommand>
{
    private readonly IApplicationDbContext context;

    public UpdateAssessmentMarkCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(v => v.AssessmentId).NotEmpty();
        RuleFor(v => v.StudentId).NotEmpty();

        this.context = context;
    }

}

public class UpdateAssessmentMarkCommandHandler : IRequestHandler<UpdateAssessmentMarkCommand>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public UpdateAssessmentMarkCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateAssessmentMarkCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.AssessmentMarks
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(AssessmentMark), request.Id);
        }

        entity = mapper.Map(request, entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

