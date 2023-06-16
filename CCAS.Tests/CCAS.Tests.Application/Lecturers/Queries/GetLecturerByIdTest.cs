using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Interfaces;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Lecturers.Queries;
using FluentAssertions;
using Moq;

public class GetLecturerByIdTest : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public GetLecturerByIdTest(DbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_GivenValidId_ShouldReturnCorrectLecturerVm()
    {
        var options = _fixture.CreateNewContextOptions();
        _fixture.Seed(options);

        var query = new GetLecturerById { Id = 1 };
        var sut = new GetLecturerByIdQueryHandler(new ApplicationDbContext(options, Mock.Of<ICurrentUserService>(), Mock.Of<IDateTime>()), _fixture.Mapper);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().BeOfType<LecturerVM>();
        result.Id.Should().Be(query.Id);
        result.Name.Should().Be("Test Lecturer 1");
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ShouldThrowNotFoundException()
    {
        var options = _fixture.CreateNewContextOptions();
        _fixture.Seed(options);

        var query = new GetLecturerById { Id = 99 };
        var sut = new GetLecturerByIdQueryHandler(new ApplicationDbContext(options, Mock.Of<ICurrentUserService>(), Mock.Of<IDateTime>()), _fixture.Mapper);

        Func<Task> act = async () => { await sut.Handle(query, CancellationToken.None); };

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
