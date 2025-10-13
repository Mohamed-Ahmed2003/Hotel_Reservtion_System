using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Hotel_Reservtion_System.CastuomValidation
{
    public class RoleValidation: ValidationAttribute
    {
        private enum Roles
        {
            Admin,
            User,
            Employee
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                PropertyInfo? property = validationContext.ObjectType.GetProperty("role");
                if (property != null)
                {
                    string roleValue = property.GetValue(validationContext.ObjectInstance)?.ToString() ?? string.Empty;
                    for(int i = 0;i< Enum.GetNames(typeof(Roles)).Length; i++)
                    {
                        if (string.Equals(roleValue, Enum.GetNames(typeof(Roles))[i], StringComparison.OrdinalIgnoreCase))
                        {
                            return ValidationResult.Success;
                        }
                    }

                }
                return new ValidationResult("Role property not found.");

            }
            return new ValidationResult("Role is required.");
        }
    }
}
