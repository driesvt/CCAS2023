using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.Companies.Queries;
public class CompanyComplexVM : IMapFrom<Company>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? PostalAddress { get; set; }
    public DateTime InceptionDate { get; set; }

    public int CompanyAge { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Company, CompanyComplexVM>()
            .ForMember(cdm => cdm.CompanyAge, opt => opt.MapFrom(c => DateTime.Now.Year - c.InceptionDate.Year));
    }
    
}
