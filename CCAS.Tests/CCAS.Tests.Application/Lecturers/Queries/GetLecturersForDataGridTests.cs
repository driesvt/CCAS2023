using CCAS.Application.Common.Interfaces;
using CCAS.Application.Common.Models;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Lecturers.Queries;
using FluentAssertions;
using Moq;

public class GetLecturersForDataGridTests : IClassFixture<DbFixture>
{
    private readonly DbFixture _fixture;

    public GetLecturersForDataGridTests(DbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnPaginatedLecturersList()
    {
        var options = _fixture.CreateNewContextOptions();
        _fixture.Seed(options);

        var query = new GetLecturersForDataGrid { Skip = 1, Take = 2 };
        var sut = new GetLecturersForDataGridHandler(new ApplicationDbContext(options, Mock.Of<ICurrentUserService>(), Mock.Of<IDateTime>()), _fixture.Mapper);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().BeOfType<PaginatedList<LecturerVM>>();
        result.TotalCount.Should().Be(3);
        result.Items.Count.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WhenCalledWithFilter_ShouldReturnFilteredLecturersList()
    {
        var options = _fixture.CreateNewContextOptions();
        _fixture.Seed(options);

        var query = new GetLecturersForDataGrid { Skip = 0, Take = 10, Name = "Test Lecturer 1" };
        var sut = new GetLecturersForDataGridHandler(new ApplicationDbContext(options, Mock.Of<ICurrentUserService>(), Mock.Of<IDateTime>()), _fixture.Mapper);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().BeOfType<PaginatedList<LecturerVM>>();
        result.TotalCount.Should().Be(1);
        result.Items.First().Name.Should().Be("Test Lecturer 1");
    }
}
