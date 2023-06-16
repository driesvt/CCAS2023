using Microsoft.EntityFrameworkCore;
using CCAS.Application.Common.Interfaces;
using System.Reflection;
using CCAS.Application.Common.Entities;

namespace CCAS.Application.Common.Persistence;

public interface IApplicationDbContext
{
    DbSet<Company> Companies { get; }
    DbSet<Lecturer> Lecturers { get; }
    Task<Lecturer> FindLecturerAsync(int id, CancellationToken cancellationToken);
    DbSet<Student> Students { get; }
    DbSet<Subject> Subjects { get; }
    DbSet<Assessment> Assessments { get; }
    DbSet<AssessmentMark> AssessmentMarks { get; }
    DbSet<SSJoin> SSJoins { get; }
    DbSet<LSuJoin> LSuJoins { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApplicationDbContext(
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService,
        IDateTime dateTime)
        : base(options)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public async Task<Lecturer> FindLecturerAsync(int id, CancellationToken cancellationToken)
    {
        return await Lecturers.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<AssessmentMark> AssessmentMarks { get; set; }
    public DbSet<SSJoin> SSJoins { get; set; }
    public DbSet<LSuJoin> LSuJoins { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserName;
                    entry.Entity.Created = _dateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserName;
                    entry.Entity.LastModified = _dateTime.Now;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

}
