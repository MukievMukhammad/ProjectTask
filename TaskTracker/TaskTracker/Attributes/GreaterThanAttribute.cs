using System;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Attributes
{
    public sealed class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string testedPropertyName;
        private readonly bool allowEqualDates;

        public GreaterThanAttribute(string testedPropertyName, bool allowEqualDates = false)
        {
            this.testedPropertyName = testedPropertyName;
            this.allowEqualDates = allowEqualDates;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(testedPropertyName);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult("Incorrect Date");
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            if (value == null || !(value is DateTime))
            {
                return ValidationResult.Success;
            }

            if (propertyTestedValue == null || !(propertyTestedValue is DateTime))
            {
                return ValidationResult.Success;
            }

            // Compare values
            if ((DateTime)value >= (DateTime)propertyTestedValue)
            {
                if (allowEqualDates && value == propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
                else if ((DateTime)value >= (DateTime)propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Start Date Should be less than Complete");
        }
    }
}