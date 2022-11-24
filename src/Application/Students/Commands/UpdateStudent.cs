using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Students.Commands;

public class UpdateStudentCommand : IRequest, IMapFrom<Student>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    //public string? ImageTitle { get; set; }
    //public byte[]? ImageData { get; set; }
    public string? StudentNumber { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? PostalAddress { get; set; }
    public int? Year { get; set; }
    public string? Imagesrc { get; set; }
    public DateTime InceptionDate { get; set; }
}

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    private readonly IApplicationDbContext context;

    public UpdateStudentCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(StudentNameMustNotExist).WithMessage("A student with this name already exists");
        RuleFor(v => v.StudentNumber).MustAsync(StudentNumberMustNotExist).WithMessage("A student with this number already exists");

        this.context = context;
    }

    async Task<bool> StudentNameMustNotExist(UpdateStudentCommand command, string? studentName, CancellationToken cancellationToken)
    {
        return await context.Students.CountAsync(p => p.Name == studentName && p.Id != command.Id) == 0 ? true : false;
    }
    async Task<bool> StudentNumberMustNotExist(UpdateStudentCommand command, string? studentNumber, CancellationToken cancellationToken)
    {
        return await context.Students.CountAsync(p => p.StudentNumber == studentNumber && p.Id != command.Id) == 0 ? true : false;
    }
}

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public UpdateStudentCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Students
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Student), request.Id);
        }

        entity = mapper.Map(request, entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

