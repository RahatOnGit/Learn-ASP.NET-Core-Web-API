using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Validators
{
    public class UpperCaseName : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var data = (string?) value;

            if (data[0]>='A' && data[0]<='Z')
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Name must start with uppercase alphabet letter!");
        }
    }
}
