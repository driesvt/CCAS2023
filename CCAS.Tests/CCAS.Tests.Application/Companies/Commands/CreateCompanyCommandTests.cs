using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Companies.Commands;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace CCAS.Tests.Application.Companies.Commands;
public class CreateCompanyCommandTests : IDisposable
{
    private readonly Mock<IApplicationDbContext> mockDbContext;
    private readonly CreateCompanyCommandValidator validator;
    private readonly CreateCompanyCommandHandler handler;

    public CreateCompanyCommandTests()
    {
        var companies = new List<Company> { new Company { Name = "Existing Company" } }.AsQueryable();
        mockDbContext = new Mock<IApplicationDbContext>();
        mockDbContext.Setup(x => x.Companies).ReturnsDbSet(companies);

        validator = new CreateCompanyCommandValidator(mockDbContext.Object);
        handler = new CreateCompanyCommandHandler(mockDbContext.Object);
    }

    public void Dispose()
    {
        // Cleanup after each test
    }

    [Theory]
    [InlineData("New Company", "invalid")] // invalid: invalid email
    [InlineData(null, "test@example.com")] // invalid: null name
    [InlineData("", "test@example.com")] // invalid: empty name
    [InlineData("Existing Company", "test@example.com")] // invalid: duplicate name
    public async Task Validator_Should_Fail_If_NameOrEmail_Is_Invalid(string name, string email)
    {
        // Arrange
        var command = new CreateCompanyCommand { Name = name, Email = email };

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        var nameErrorExists = result.Errors.Any(e => e.PropertyName == nameof(command.Name));
        var emailErrorExists = result.Errors.Any(e => e.PropertyName == nameof(command.Email));

        Assert.True(nameErrorExists || emailErrorExists, "Error should exist for either Name or Email (or both)");
    }

    [Fact]
    public async Task Validate_ValidDetails_PassesValidation()
    {
        // Arrange
        var command = new CreateCompanyCommand { Name = "New Company", Email = "test@example.com" };

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsCompanyToDatabase()
    {
        // Arrange
        var command = new CreateCompanyCommand
        {
            Name = "Test Company",
            ContactNumber = "123456",
            Email = "test@example.com",
            Website = "www.example.com",
            PhysicalAddress = "123 Test St",
            PostalAddress = "123 Test St"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        mockDbContext.Verify(db => db.Companies.Add(It.IsAny<Company>()), Times.Once());
        mockDbContext.Verify(db => db.SaveChangesAsync(CancellationToken.None), Times.Once());
    }
}
