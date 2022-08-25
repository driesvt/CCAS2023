namespace CCAS.Application.Common.Entities;

public abstract class AuditableEntity
{
    public int Id { get; set; }
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}
