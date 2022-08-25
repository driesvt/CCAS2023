using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCAS.Application.Common.Entities;
public class AssessmentMark : AuditableEntity
{
    public int AssessmentId { get; set; }
    public int StudentId { get; set; }
    public int? Mark { get; set; }

    public Assessment? Assessment { get; set; }
    public Student? Student { get; set; }
}

public class AssessmentMarkConfiguration : IEntityTypeConfiguration<AssessmentMark>
{
    public void Configure(EntityTypeBuilder<AssessmentMark> builder)
    {

    }
}


