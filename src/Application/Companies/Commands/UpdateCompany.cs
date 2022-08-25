using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Companies.Commands;

public class UpdateCompanyCommand : IRequest, IMapFrom<Company>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? PostalAddress { get; set; }
}

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    private readonly IApplicationDbContext context;

    public UpdateCompanyCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(CompanyNameMustNotExist).WithMessage("A company with this name already exists");
        RuleFor(v => v.Email).EmailAddress();

        this.context = context;
    }

    async Task<bool> CompanyNameMustNotExist(UpdateCompanyCommand command, string? companyName, CancellationToken cancellationToken)
    {
        return await context.Companies.CountAsync(p => p.Name == companyName && p.Id != command.Id) == 0 ? true : false;
    }
}

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public UpdateCompanyCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Companies
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Company), request.Id);
        }

        entity = mapper.Map(request, entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

