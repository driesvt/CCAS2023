using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Interfaces;
using CCAS.Application.Common.Mappings;
using CCAS.Application.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class DbFixture
{
    public IMapper Mapper { get; private set; }

    public DbFixture()
    {
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        Mapper = configurationProvider.CreateMapper();
    }

    public DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        return options;
    }

    public void Seed(DbContextOptions<ApplicationDbContext> options)
    {
        using var context = new ApplicationDbContext(options, Mock.Of<ICurrentUserService>(), Mock.Of<IDateTime>());
        context.Lecturers.AddRange(
            new Lecturer { Id = 1, Name = "Test Lecturer 1" },
            new Lecturer { Id = 2, Name = "Test Lecturer 2" },
            new Lecturer { Id = 3, Name = "Test Lecturer 3" }
        );

        context.SaveChanges();
    }
}
