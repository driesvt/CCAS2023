using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.AssessmentMarks.Queries;
public class AssessmentMarkComplexVM : IMapFrom<AssessmentMark>
{
    public int Id { get; set; }
    public int AssessmentId { get; set; }
    public int StudentId { get; set; }
    public int? Mark { get; set; }

    public Assessment? Assessment { get; set; }
    public Student? Student { get; set; }
}
