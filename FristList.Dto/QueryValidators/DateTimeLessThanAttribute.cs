using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FristList.Dto.QueryValidators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeLessThanAttribute : ValidationAttribute
    {
        public string OtherProperty { get; }

        public DateTimeLessThanAttribute(string otherPropertyName) 
            : base($"The field {{0}} must be less than the field {otherPropertyName}")
        {
            OtherProperty = otherPropertyName ??
                            throw new ArgumentException("otherPropertyName have to valid property name");
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);

            if (otherPropertyInfo is null)
                return new ValidationResult($"The field {OtherProperty} not found");

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (value is not DateTime dateTime)
                return new ValidationResult($"The field {{0}} must be System.DateTime");

            if (otherPropertyValue is not DateTime otherDateTime)
                return new ValidationResult($"The field {OtherProperty} must be System.DateTime");

            if (dateTime >= otherDateTime)
                return new ValidationResult($"The field {{0}} must be less than the field {OtherProperty}");

            return null;
        }
    }
}