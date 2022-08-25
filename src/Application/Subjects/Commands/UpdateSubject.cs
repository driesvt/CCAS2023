using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Subjects.Commands;

public class UpdateSubjectCommand : IRequest, IMapFrom<Subject>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Course { get; set; } // FP, IP, SP, FPandSP etc
    public string? Credits { get; set; }
    public string? MethodofDelivery { get; set; }
    public string? NQFLevel { get; set; }
    public string? Year { get; set; }
    public string? Semester { get; set; }
}

public class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
{
    private readonly IApplicationDbContext context;

    public UpdateSubjectCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(SubjectNameMustNotExist).WithMessage("A subject with this name already exists");
        RuleFor(v => v.Code).MustAsync(SubjectCodeMustNotExist).WithMessage("A subject with this code already exists");

        this.context = context;
    }

    async Task<bool> SubjectNameMustNotExist(UpdateSubjectCommand command, string? subjectName, CancellationToken cancellationToken)
    {
        return await context.Subjects.CountAsync(p => p.Name == subjectName && p.Id != command.Id) == 0 ? true : false;
    }
    async Task<bool> SubjectCodeMustNotExist(UpdateSubjectCommand command, string? subjectCode, CancellationToken cancellationToken)
    {
        return await context.Subjects.CountAsync(p => p.Code == subjectCode && p.Id != command.Id) == 0 ? true : false;
    }
}

public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public UpdateSubjectCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Subjects
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Subject), request.Id);
        }

        entity = mapper.Map(request, entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

