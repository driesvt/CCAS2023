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
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(LecturerNameMustNotExist).WithMessage("A lecturer with this name already exists");
        RuleFor(v => v.LecturerNumber).MustAsync(LecturerNumberMustNotExist).WithMessage("A lecturer with this number already exists");

        this.context = context;
    }

    async Task<bool> LecturerNameMustNotExist(UpdateLecturerCommand command, string? lecturerName, CancellationToken cancellationToken)
    {
        return await context.Lecturers.CountAsync(p => p.Name == lecturerName && p.Id != command.Id) == 0 ? true : false;
    }
    async Task<bool> LecturerNumberMustNotExist(UpdateLecturerCommand command, string? lecturerNumber, CancellationToken cancellationToken)
    {
        return await context.Lecturers.CountAsync(p => p.LecturerNumber == lecturerNumber && p.Id != command.Id) == 0 ? true : false;
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
        var entity = await context.Lecturers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Lecturer), request.Id);
        }

        entity = mapper.Map(request, entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

