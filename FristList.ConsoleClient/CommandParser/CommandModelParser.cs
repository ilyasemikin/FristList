using System.Reflection;
using FristList.Common.Deserializers;
using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.CommandParser.Attributes;
using FristList.ConsoleClient.CommandParser.Exceptions;
using FristList.ConsoleClient.Commands;

namespace FristList.ConsoleClient.CommandParser;

public class CommandModelParser : ICommandModelParser
{
    private readonly HashSet<Type> _attributes = new()
    {
        typeof(PositionalAttribute),
        typeof(OptionalAttribute)
    };

    public TCommandModel Parse<TCommandModel>(Parameters parameters) where TCommandModel : CommandModelBase
    { 
        var properties = typeof(TCommandModel).GetProperties()
            .Where(p => p.CustomAttributes.Any(d => _attributes.Contains(d.AttributeType)));

        var positionals = new List<(PropertyInfo property, PositionalAttribute attribute)>();
        var optionals = new List<(PropertyInfo property, OptionalAttribute attribute)>();

        foreach (var property in properties)
        {
            foreach (var attribute in property.GetCustomAttributes())
            {
                switch (attribute)
                {
                    case PositionalAttribute positionalAttribute:
                        positionals.Add((property, positionalAttribute));
                        break;
                    case OptionalAttribute optionalAttribute:
                        optionals.Add((property, optionalAttribute));
                        break;
                    default:
                        continue;
                }

                break;
            }
        }
        
        var model = (TCommandModel)Activator.CreateInstance(typeof(TCommandModel))!;

        if (positionals.Count == 0)
            return model;
        
        var expectedParametersCount = positionals.Max(pair => pair.attribute.Position) + 1;
        if (parameters.Positional.Count < expectedParametersCount)
            throw new ParametersMissingException(expectedParametersCount, parameters.Positional.Count);
        if (parameters.Positional.Count > expectedParametersCount)
            throw new ParametersRedundantException(expectedParametersCount, parameters.Positional.Count);

        FillModelValues(model, positionals,
            (type, attr) => StringDeserializers.Deserialize(parameters.Positional[attr.Position], type));
        FillModelValues(model, optionals,
            (type, attr) => StringDeserializers.Deserialize(parameters.Optional[attr.Name], type));

        return model;
    }

    private void FillModelValues<TCommandModel, TAttribute>(
        TCommandModel model,
        IEnumerable<(PropertyInfo property, TAttribute attribute)> props, 
        Func<Type, TAttribute, object?> valueSelector)
    {
        foreach (var (property, attribute) in props)
        {
            var type = property.PropertyType;
            var value = valueSelector(type, attribute);
            property.SetValue(model, value);
        }
    }
}