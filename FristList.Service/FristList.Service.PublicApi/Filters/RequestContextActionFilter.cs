using FristList.Service.PublicApi.Context;
using JetBrains.Annotations;

namespace FristList.Service.PublicApi.Filters;

[UsedImplicitly]
public class RequestContextActionFilter : RequestContextActionFilterBase
{
    protected override IEnumerable<Variable> GetVariables()
    {
        var fields = typeof(RequestContextVariables)
            .GetFields()
            .Where(f => f.FieldType.BaseType == typeof(Variable))
            .ToArray();
        return fields.Select(f => f.GetValue(null) as Variable)
            .OfType<Variable>();
    }
}