using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.SSJoins.Queries;

public class SSJoinVM : IMapFrom<SSJoin>
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }

    public Student? Student { get; set; }
    public Subject? Subject { get; set; }

}
