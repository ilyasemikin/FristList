using System.Threading.Tasks;
using FristList.Data.Models.Base;

namespace FristList.Services.Abstractions;

public interface IModelLinkPropertyAggregator
{
    Task FillPropertiesAsync(ModelObjectBase model);
}