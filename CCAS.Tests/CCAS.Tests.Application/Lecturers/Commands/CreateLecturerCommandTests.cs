using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baseline;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Companies.Commands;
using CCAS.Application.Lecturers.Commands;
using Moq;
using Moq.EntityFrameworkCore;

namespace CCAS.Tests.Application.Lecturers.Commands;
public class CreateLecturerCommandTests : IDisposable
{
    private readonly Mock<IApplicationDbContext> mockDbContext;
    private readonly CreateLecturerCommandValidator validator;
    private readonly CreateLecturerCommandHandler handler;

    public CreateLecturerCommandTests()
    {
        var lecturers = new List<Lecturer> { new Lecturer { Name = "Existing Lecturer", LecturerNumber = "Existing Number" } }.AsQueryable();
        mockDbContext = new Mock<IApplicationDbContext>();
        mockDbContext.Setup(x => x.Lecturers).ReturnsDbSet(lecturers);

        validator = new CreateLecturerCommandValidator(mockDbContext.Object);
        handler = new CreateLecturerCommandHandler(mockDbContext.Object);
    }

    public void Dispose()
    {
        // Cleanup after each test
    }

    [Theory]
    [InlineData("New Lecturer", "invalid", "New Number")] // invalid: invalid email
    [InlineData(null, "test@example.com", "New Number")] // invalid: null name
    [InlineData("", "test@example.com", "New Number")] // invalid: empty name
    [InlineData("Existing Lecturer", "test@example.com", "New Number")] // invalid: duplicate name
    [InlineData("New Lecturer", "test@example.com", null)] // invalid: null Lecturer Number
    [InlineData("New Lecturer", "test@example.com", "")] // invalid: empty Lecturer Number
    [InlineData("New Lecturer", "test@example.com", "Existing Number")] // invalid: duplicate Lecturer Number
    public async Task Validator_Should_Fail_If_NameOrEmailOrLecturerNumber_Is_Invalid(string name, string email, string lecturernumber)
    {
        // Arrange
        var command = new CreateLecturerCommand { Name = name, Email = email, LecturerNumber = lecturernumber };

        // Act
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        // Assert
        var nameErrorExists = result.Errors.Any(e => e.PropertyName == nameof(command.Name));
        var emailErrorExists = result.Errors.Any(e => e.PropertyName == nameof(command.Email));
        var lecturernumberErrorExists = result.Errors.Any(e => e.PropertyName == nameof(command.LecturerNumber));

        Assert.True(nameErrorExists || emailErrorExists || lecturernumberErrorExists, "Error should exist for either Name or Email or LecturerNumber (or more)");
    }

    [Fact]
    public async Task Validate_ValidDetails_PassesValidation()
    {
        // Arrange
        var command = new CreateLecturerCommand { Name = "New Lecturer", Email = "test@example.com", LecturerNumber = "New Number" };

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsLecturerToDatabase()
    {
        // Arrange
        var command = new CreateLecturerCommand
        {
            Name = "Test Lecturer",
            ContactNumber = "123456",
            Email = "test1@example.com",
            PhysicalAddress = "123 Test St",
            PostalAddress = "123 Test St"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        mockDbContext.Verify(db => db.Lecturers.Add(It.IsAny<Lecturer>()), Times.Once());
        mockDbContext.Verify(db => db.SaveChangesAsync(CancellationToken.None), Times.Once());
    }
}
