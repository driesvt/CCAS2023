using CCAS.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace CCAS.Application.Common.Models;

public class SortOption<TModel>
{
    public Expression<Func<TModel, object>> KeySelector { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

    public SortOption(Expression<Func<TModel, object>> keySelector, SortOrder sortOrder = SortOrder.Ascending)
    {
        this.KeySelector = keySelector;
        this.SortOrder = sortOrder;
    }
}