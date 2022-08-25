using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Companies.Commands;


public class CreateCompanyCommand : IRequest<int>
{
    public string? Name { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? PostalAddress { get; set; }
}

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    private readonly IApplicationDbContext context;

    public CreateCompanyCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(CompanyNameMustNotExist).WithMessage("A company with this name already exists");
        RuleFor(v => v.Email).EmailAddress();
        this.context = context;
    }

    async Task <bool> CompanyNameMustNotExist(string? companyName, CancellationToken cancellationToken)
    {
        return await context.Companies.CountAsync(p => p.Name == companyName) == 0 ? true : false;
    }
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, int>
{
    private readonly IApplicationDbContext context;

    public CreateCompanyCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = new Company
        {
            Name = request.Name,
            ContactNumber = request.ContactNumber,
            Email = request.Email,
            Website = request.Website,
            PhysicalAddress = request.PhysicalAddress,
            PostalAddress = request.PostalAddress
        };

        context.Companies.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}


