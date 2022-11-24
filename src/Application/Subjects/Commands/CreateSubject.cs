using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Subjects.Commands;


public class CreateSubjectCommand : IRequest<int>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Course { get; set; } // FP, IP, SP, FPandSP etc
    public string? Credits { get; set; }
    public string? MethodofDelivery { get; set; }
    public string? NQFLevel { get; set; }
    public string? Year { get; set; }
    public string? Semester { get; set; }
    public string? Imagesrc { get; set; }
}

public class CreateSubjectCommandValidator : AbstractValidator<CreateSubjectCommand>
{
    private readonly IApplicationDbContext context;

    public CreateSubjectCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(SubjectNameMustNotExist).WithMessage("A subject with this name already exists");
        RuleFor(v => v.Code).MustAsync(SubjectCodeMustNotExist).WithMessage("A subject with this code already exists");
        this.context = context;
    }

    async Task <bool> SubjectNameMustNotExist(string? subjectName, CancellationToken cancellationToken)
    {
        return await context.Subjects.CountAsync(p => p.Name == subjectName) == 0 ? true : false;
    }
    async Task<bool> SubjectCodeMustNotExist(string? subjectCode, CancellationToken cancellationToken)
    {
        return await context.Subjects.CountAsync(p => p.Code == subjectCode) == 0 ? true : false;
    }
}

public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, int>
{
    private readonly IApplicationDbContext context;

    public CreateSubjectCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<int> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var entity = new Subject
        {
            Name = request.Name,
            Code = request.Code,
            Course = request.Course,
            Credits = request.Credits,
            MethodofDelivery = request.MethodofDelivery,
            NQFLevel = request.NQFLevel,
            Year = request.Year,
            Semester = request.Semester,
            Imagesrc = request.Imagesrc
        };

        context.Subjects.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


