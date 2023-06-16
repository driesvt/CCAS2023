using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Moq;
using Moq.EntityFrameworkCore;
using AutoMapper;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Lecturers.Commands;
using CCAS.Application.Common.Entities;

namespace CCAS.Tests.Application.Lecturers.Commands
{
    public class UpdateLecturerCommandTests
    {
        private readonly Mock<IApplicationDbContext> mockDbContext;
        private readonly UpdateLecturerCommandValidator validator;
        private readonly UpdateLecturerCommandHandler handler;

        public UpdateLecturerCommandTests()
        {
            var lecturer = new Lecturer
            {
                Id = 1,
                Name = "Existing Lecturer",
                LecturerNumber = "Existing Number",
                InceptionDate = DateTime.Now
            };
            var lecturers = new List<Lecturer> { lecturer };

            mockDbContext = new Mock<IApplicationDbContext>();
            mockDbContext.Setup(x => x.Lecturers).ReturnsDbSet(lecturers);
            mockDbContext.Setup(x => x.FindLecturerAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((int id, CancellationToken token) => lecturers.FirstOrDefault(l => l.Id == id));

            validator = new UpdateLecturerCommandValidator(mockDbContext.Object);
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = configurationProvider.CreateMapper();
            handler = new UpdateLecturerCommandHandler(mockDbContext.Object, mapper);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldUpdateLecturerProperties()
        {
            // Arrange
            var command = new UpdateLecturerCommand
            {
                Id = 1,
                Name = "New Name",
                LecturerNumber = "New Number"
            };

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedLecturer = mockDbContext.Object.Lecturers.Single(l => l.Id == 1);
            Assert.Equal("New Name", updatedLecturer.Name);
            Assert.Equal("New Number", updatedLecturer.LecturerNumber);
            mockDbContext.Verify(db => db.SaveChangesAsync(CancellationToken.None), Times.Once());
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new UpdateLecturerCommand
            {
                Id = 2, // This lecturer does not exist
                Name = "New Name",
                LecturerNumber = "New Number"
            };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
