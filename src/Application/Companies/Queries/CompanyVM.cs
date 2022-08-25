using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Mappings;

namespace CCAS.Application.Companies.Queries;

public class CompanyVM : IMapFrom<Company>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? PhysicalAddress { get; set; }
    public string? PostalAddress { get; set; }


}
