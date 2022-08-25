﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.Students.Queries;
public class StudentComplexVM : IMapFrom<Student>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? StudentNumber { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? PostalAddress { get; set; }
    public DateTime InceptionDate { get; set; }

    public ICollection<AssessmentMark>? AssessmentMarks { get; set; }
    public ICollection<Subject>? Subjects { get; set; }
}
