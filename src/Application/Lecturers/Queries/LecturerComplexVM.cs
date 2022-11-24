using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.Lecturers.Queries;
public class LecturerComplexVM : IMapFrom<Lecturer>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? LecturerNumber { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? PostalAddress { get; set; }
    public string? Imagesrc { get; set; }
    public DateTime InceptionDate { get; set; }

    public ICollection<Subject>? Subjects { get; set; }
}
