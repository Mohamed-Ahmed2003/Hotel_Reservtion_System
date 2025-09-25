using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Hotel_Reservtion_System.CastuomValidation
{
    public class RoleValidation: ValidationAttribute
    {
       protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                PropertyInfo? property = validationContext.ObjectType.GetProperty("role");
                if (property != null)
                {
                    string roleValue = property.GetValue(validationContext.ObjectInstance)?.ToString() ?? string.Empty;
                    
                    if (roleValue.ToLower() == "admin" || roleValue.ToLower() == "user"|| roleValue.ToLower() == "employee")
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("Role must be either 'Admin' or 'User'.");
                    }
                }
                return new ValidationResult("Role property not found.");

            }
        }
    }
}
