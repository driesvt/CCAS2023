using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCAS.Application.Common.Entities;
public class Subject : AuditableEntity
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Course { get; set; } // FP, IP, SP, FPandSP etc
    public string? Credits { get; set; }
    public string? MethodofDelivery { get; set; }
    public string? NQFLevel { get; set; }
    public string? Year { get; set; }
    public string? Semester { get; set; }
    public string? Imagesrc { get; set; }

    public ICollection<SSJoin>? SSJoins { get; set; }
    public ICollection<LSuJoin>? LSuJoins { get; set; }
    public ICollection<Assessment>? Assessments { get; set; }
}

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {

    }
}


