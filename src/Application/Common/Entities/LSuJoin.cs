using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCAS.Application.Common.Entities;
public class LSuJoin : AuditableEntity
{
    public int LecturerId { get; set; }
    public int SubjectId { get; set; }

    public Lecturer? Lecturer { get; set; }
    public Subject? Subject { get; set; }
}

public class LSuJoinConfiguration : IEntityTypeConfiguration<LSuJoin>
{
    public void Configure(EntityTypeBuilder<LSuJoin> builder)
    {

    }
}


