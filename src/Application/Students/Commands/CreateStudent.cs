using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Students.Commands;


public class CreateStudentCommand : IRequest<int>
{
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

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    private readonly IApplicationDbContext context;

    public CreateStudentCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(StudentNameMustNotExist).WithMessage("A student with this name already exists");
        RuleFor(v => v.StudentNumber).MustAsync(StudentNumberMustNotExist).WithMessage("A student with this number already exists");
        this.context = context;
    }

    async Task <bool> StudentNameMustNotExist(string? studentName, CancellationToken cancellationToken)
    {
        return await context.Students.CountAsync(p => p.Name == studentName) == 0 ? true : false;
    }
    async Task<bool> StudentNumberMustNotExist(string? studentNumber, CancellationToken cancellationToken)
    {
        return await context.Students.CountAsync(p => p.StudentNumber == studentNumber) == 0 ? true : false;
    }
}

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, int>
{
    private readonly IApplicationDbContext context;

    public CreateStudentCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Student
        {
            Name = request.Name,
            //ImageTitle = request.ImageTitle,
            //ImageData = request.ImageData,
            StudentNumber = request.StudentNumber,
            ContactNumber = request.ContactNumber,
            Email = request.Email,
            PhysicalAddress = request.PhysicalAddress,
            PostalAddress = request.PostalAddress,
            Year = request.Year,
            Imagesrc = request.Imagesrc,
            InceptionDate = request.InceptionDate
        };

        context.Students.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


