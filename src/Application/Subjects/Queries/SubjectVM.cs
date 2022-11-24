using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.Subjects.Queries;

public class SubjectVM : IMapFrom<Subject>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Course { get; set; } // FP, IP, SP, FPandSP etc
    public string? Credits { get; set; }
    public string? MethodofDelivery { get; set; }
    public string? NQFLevel { get; set; }
    public string? Year { get; set; }
    public string? Semester { get; set; }
    public string? Imagesrc { get; set; }

    public ICollection<Lecturer>? Lecturers { get; set; }
    public ICollection<Student>? Students { get; set; }
    public ICollection<Assessment>? Assessments { get; set; }

}
