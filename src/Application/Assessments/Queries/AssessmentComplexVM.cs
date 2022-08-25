using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.Assessments.Queries;
public class AssessmentComplexVM : IMapFrom<Assessment>
{
    public int Id { get; set; }
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
