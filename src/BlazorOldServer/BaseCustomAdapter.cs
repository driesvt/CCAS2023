using MediatR;
using Syncfusion.Blazor;

namespace CCAS.BlazorOldServer;

public class BaseCustomAdapter : DataAdaptor
{
    protected readonly IMediator Mediator;

    public BaseCustomAdapter(IMediator mediator)
    {
        this.Mediator = mediator;
    }

    public virtual string? GetFieldFilter(DataManagerRequest dm, string fieldName)
    {
        if (dm.Where != null && dm.Where.Count > 0)
        {
            var predicate = dm.Where.First().predicates.FirstOrDefault(p => p.Field.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            if (predicate != null)
                return predicate.value.ToString();
            else
                return null;
        }
        else
            return null;
    }

    public virtual List<string> GetSort(DataManagerRequest dm, string defaultSortField)
    {
        if (dm.Sorted != null && dm.Sorted.Count > 0)
            return dm.Sorted.Select(p => p.Direction == "ascending" ? "+" + p.Name : "-" + p.Name).ToList();
        else
            return new List<string>() { defaultSortField };
    }
}
