using FristList.Service.Data.Models.Categories.Base;

namespace FristList.Service.PublicApi.Services.Models.Activities;

public class ActivitiesSearchParams
{
    /// <summary>
    /// 
    /// </summary>
    public IList<BaseCategory> Categories { get; set; } = new List<BaseCategory>();

    public ActivitiesSearchSortField SortField { get; set; } = ActivitiesSearchSortField.Unknown;

    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}