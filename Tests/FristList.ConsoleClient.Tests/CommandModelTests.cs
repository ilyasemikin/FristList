using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.CommandParser.Attributes;
using NUnit.Framework;

namespace FristList.ConsoleClient.Tests;

public class CommandModelTests
{
    private static readonly string AssemblyName = "FristList.ConsoleClient";
    
    [TestCaseSource(nameof(GetCommandModelCases))]
    public void CommandModelTypesShouldCorrectPositionalNumbers(Type commandModelType)
    {
        var properties = GetPropertiesWithAttribute<PositionalAttribute>(commandModelType)
            .ToList();
        var positionalAttributes = GetAttributesOfType<PositionalAttribute>(properties)
            .OrderBy(attr => attr.Position)
            .ToList();

        var positions = positionalAttributes.Select(attr => attr.Position)
            .ToList();
        var positionDiffs = new[] { -1 }.Concat(positions).Zip(positions)
            .Select(pair => pair.Second - pair.First)
            .ToList();

        properties.All(p => p.CustomAttributes.Count(attr => attr.AttributeType == typeof(PositionalAttribute)) == 1)
            .Should()
            .BeTrue();

        if (positions.Count > 0)
        {
            positions.Should().StartWith(0);
            positionDiffs.Should().AllSatisfy(x => x.Should().Be(1));
        }
    }

    [TestCaseSource(nameof(GetCommandModelCases))]
    public void CommandModelTypesShouldOptionalNamesUnique(Type commandModelType)
    {
        var properties = GetPropertiesWithAttribute<OptionalAttribute>(commandModelType)
            .ToList();
        var optionalAttributes = GetAttributesOfType<OptionalAttribute>(properties)
            .ToList();

        var optionalNamesCount = optionalAttributes.Select(attr => attr.Name)
            .GroupBy(name => name)
            .ToDictionary(g => g.Key, g => g.Count());

        properties.All(p => p.CustomAttributes.Count(attr => attr.AttributeType == typeof(OptionalAttribute)) == 1)
            .Should()
            .BeTrue();
        if (optionalAttributes.Count > 0)
            optionalNamesCount.Should().AllSatisfy(pair => pair.Value.Should().Be(1));
    }

    private static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<TAttribute>(Type type)
    {
        return type.GetProperties()
            .Where(p => p.CustomAttributes.Any(attr => attr.AttributeType == typeof(TAttribute)));
    }

    private static IEnumerable<TAttribute> GetAttributesOfType<TAttribute>(IEnumerable<PropertyInfo> properties)
    {
        return properties.Select(
                p => p.GetCustomAttributes().Single(attr => attr is TAttribute))
            .Cast<TAttribute>();
    }

    private static IEnumerable<TestCaseData> GetCommandModelCases()
    {
        var assembly = Assembly.Load(AssemblyName);

        if (assembly is null)
            throw new InvalidOperationException($"Assembly {AssemblyName} is not found");

        var types = assembly.DefinedTypes
            .Where(info => info.IsClass && info.IsAssignableTo(typeof(CommandModelBase)))
            .Select(info => new TestCaseData(info.AsType()));

        var ignoreModels = new HashSet<Type> { typeof(CommandModelBase), typeof(EmptyModel) };
        types = types.Where(t => !ignoreModels.Contains(t.Arguments[0]));
        
        return types;
    }
}