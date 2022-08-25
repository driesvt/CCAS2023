using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCAS.Application.Common.Entities;
public class SSJoin : AuditableEntity
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }

    public Student Student { get; set; }
    public Subject Subject { get; set; }
}

public class SSJoinConfiguration : IEntityTypeConfiguration<SSJoin>
{
    public void Configure(EntityTypeBuilder<SSJoin> builder)
    {

    }
}


