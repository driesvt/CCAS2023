﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CCAS.Application.Common.Entities;
public class Student : AuditableEntity
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
    public ICollection<SSJoin>? SSJoins { get; set; }
    //public int ImageId { get; set; }
    //public Image? Image { get; set; }
}

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {

    }
}


