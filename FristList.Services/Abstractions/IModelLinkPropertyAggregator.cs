using System.Threading.Tasks;
using FristList.Models.Base;

namespace FristList.Services.Abstractions;

public interface IModelLinkPropertyAggregator
{
    Task FillPropertiesAsync(ModelObjectBase model);
}