using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Lecturers.Commands;

public class UpdateLecturerCommand : IRequest, IMapFrom<Lecturer>
{
    public int Id { get; set; }
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

public class UpdateLecturerCommandValidator : AbstractValidator<UpdateLecturerCommand>
{
    private readonly IApplicationDbContext context;

    public UpdateLecturerCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(v => v.Name)
            .NotEmpty()
            .MustAsync((command, lecturerName, cancellationToken) => LecturerNameMustNotExist(command.Id, lecturerName, cancellationToken))
            .WithMessage("A lecturer with this name already exists");
        RuleFor(v => v.LecturerNumber)
            .NotEmpty()
            .MustAsync((command, lecturerNumber, cancellationToken) => LecturerNumberMustNotExist(command.Id, lecturerNumber, cancellationToken))
            .WithMessage("A lecturer with this number already exists");

        this.context = context;
    }

    async Task<bool> LecturerNameMustNotExist(int id, string? lecturerName, CancellationToken cancellationToken)
    {
        return await context.Lecturers.CountAsync(p => p.Name == lecturerName && p.Id != id) == 0;
    }
    async Task<bool> LecturerNumberMustNotExist(int id, string? lecturerNumber, CancellationToken cancellationToken)
    {
        return await context.Lecturers.CountAsync(p => p.LecturerNumber == lecturerNumber && p.Id != id) == 0;
    }
}

public class UpdateLecturerCommandHandler : IRequestHandler<UpdateLecturerCommand>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public UpdateLecturerCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateLecturerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.FindLecturerAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Lecturer), request.Id);
        }

        entity = mapper.Map(request, entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

