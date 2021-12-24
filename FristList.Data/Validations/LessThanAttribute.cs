using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FristList.Data.Validations;

[AttributeUsage(AttributeTargets.Property)]
public class LessThanAttribute : ValidationAttribute
{
    public string OtherPropertyName { get; }

    public LessThanAttribute(string otherPropertyName)
        : base($"The field {{0}} must be less than the property {otherPropertyName}")
    {
        OtherPropertyName = otherPropertyName ??
                            throw new ArgumentException("otherPropertyName have to valid property name");
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherPropertyName);
        if (otherPropertyInfo is null)
            return new ValidationResult($"Property {OtherPropertyName} not found");

        var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
        if (value is not IComparable lhs)
            return new ValidationResult($"Property {validationContext.MemberName} have to implement IComparable");
        if (otherPropertyValue is not IComparable rhs)
            return new ValidationResult($"Property {OtherPropertyName} have to implement IComparable");

        if (lhs.CompareTo(rhs) >= 0)
            return new ValidationResult($"Property {validationContext.MemberName} have to less than property {OtherPropertyName}");

        return null;
    }
}