using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.LSuJoins.Queries;
public class LSuJoinComplexVM : IMapFrom<LSuJoin>
{
    public int Id { get; set; }
    public int LecturerId { get; set; }
    public int SubjectId { get; set; }

    public Lecturer? Lecturer { get; set; }
    public Subject? Subject { get; set; }
}
