using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.Students.Queries;
public class CreateStudentVM : IMapFrom<Student>
{
    public string? Name { get; set; }
    //public string? ImageTitle { get; set; }
    //public byte[]? ImageData { get; set; }
    public string? StudentNumber { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? PostalAddress { get; set; }
    public int? Year { get; set; }
    public string? Imagesrc { get; set; }
    public DateTime InceptionDate { get; set; }

    public ICollection<AssessmentMark>? AssessmentMarks { get; set; }
    public ICollection<Subject>? Subjects { get; set; }
}
