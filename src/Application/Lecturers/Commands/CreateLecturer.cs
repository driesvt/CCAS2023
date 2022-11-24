using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Lecturers.Commands;


public class CreateLecturerCommand : IRequest<int>
{
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? LecturerNumber { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? PostalAddress { get; set; }
    public string? Imagesrc { get; set; }
    public DateTime InceptionDate { get; set; }
}

public class CreateLecturerCommandValidator : AbstractValidator<CreateLecturerCommand>
{
    private readonly IApplicationDbContext context;

    public CreateLecturerCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(LecturerNameMustNotExist).WithMessage("A lecturer with this name already exists");
        RuleFor(v => v.LecturerNumber).MustAsync(LecturerNumberMustNotExist).WithMessage("A lecturer with this number already exists");
        this.context = context;
    }

    async Task <bool> LecturerNameMustNotExist(string? lecturerName, CancellationToken cancellationToken)
    {
        return await context.Lecturers.CountAsync(p => p.Name == lecturerName) == 0 ? true : false;
    }
    async Task<bool> LecturerNumberMustNotExist(string? lecturerNumber, CancellationToken cancellationToken)
    {
        return await context.Lecturers.CountAsync(p => p.LecturerNumber == lecturerNumber) == 0 ? true : false;
    }
}

public class CreateLecturerCommandHandler : IRequestHandler<CreateLecturerCommand, int>
{
    private readonly IApplicationDbContext context;

    public CreateLecturerCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<int> Handle(CreateLecturerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Lecturer
        {
            Name = request.Name,
            Username = request.Username,
            LecturerNumber = request.LecturerNumber,
            ContactNumber = request.ContactNumber,
            Email = request.Email,
            PhysicalAddress = request.PhysicalAddress,
            PostalAddress = request.PostalAddress,
            Imagesrc = request.Imagesrc,
            InceptionDate = request.InceptionDate
        };

        context.Lecturers.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


