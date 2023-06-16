using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Lecturers.Commands;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace CCAS.Tests.Application.Lecturers.Commands;
public class DeleteLecturerCommandTests
{
    private Mock<IApplicationDbContext> mockDbContext;
    private DeleteLecturerCommandHandler handler;

    public DeleteLecturerCommandTests()
    {
        mockDbContext = new Mock<IApplicationDbContext>();

        handler = new DeleteLecturerCommandHandler(mockDbContext.Object);
    }

    [Fact]
    public async Task Handler_Should_Delete_Lecturer_If_It_Exists()
    {
        // Arrange
        var command = new DeleteLecturerCommand { Id = 1 };
        var data = new List<Lecturer> { new Lecturer { Id = 1 } }.AsQueryable();

        mockDbContext.Setup(x => x.Lecturers).ReturnsDbSet(data);

        // Act
        await handler.Handle(command, default);

        // Assert
        mockDbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handler_Should_Throw_Exception_If_Lecturer_Does_Not_Exist()
    {
        // Arrange
        var command = new DeleteLecturerCommand { Id = 2 };
        var data = new List<Lecturer> { new Lecturer { Id = 1 } }.AsQueryable();

        mockDbContext.Setup(x => x.Lecturers).ReturnsDbSet(data);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, default));
    }
}
