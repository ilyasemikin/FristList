using FristList.Service.Data.Models.Categories.Base;

namespace FristList.Service.PublicApi.Services.Models.Activities;

public class ActivitiesSearchParams
{
    /// <summary>
    /// 
    /// </summary>
    public IList<BaseCategory> Categories { get; set; } = new List<BaseCategory>();

    public ActivitiesSearchOrder Order { get; set; } = ActivitiesSearchOrder.Unknown;
}