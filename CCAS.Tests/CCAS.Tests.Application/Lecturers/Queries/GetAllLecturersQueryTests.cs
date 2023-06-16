using CCAS.Application.Common.Interfaces;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Lecturers.Queries;
using FluentAssertions;
using Moq;

public class GetAllLecturersQueryTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public GetAllLecturersQueryTests(DbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnLecturersList()
    {
        var options = _fixture.CreateNewContextOptions();
        _fixture.Seed(options);

        var sut = new GetAllLecturersQueryHandler(new ApplicationDbContext(options, Mock.Of<ICurrentUserService>(), Mock.Of<IDateTime>()), _fixture.Mapper);

        var result = await sut.Handle(new GetAllLecturersQuery(), CancellationToken.None);

        result.Should().BeOfType<List<LecturerVM>>();
        result.Count.Should().Be(3);
    }
}
