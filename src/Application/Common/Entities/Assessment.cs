using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCAS.Application.Common.Entities;
public class Assessment : AuditableEntity
{
    public string? Name { get; set; }
    public string? AssessmentCode { get; set; }
    public string? Author { get; set; }
    public string? Moderator { get; set; }
    public string? Details { get; set; }
    public int? MaxMark { get; set; }
    public string? Weighting { get; set; }
    public DateTime ModerationSubmitDate { get; set; }
    public DateTime ModerationCompleteDate { get; set; }
    public DateTime DueDate { get; set; }

    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }
    public ICollection<AssessmentMark>? AssessmentMarks { get; set; }
}

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {

    }
}


