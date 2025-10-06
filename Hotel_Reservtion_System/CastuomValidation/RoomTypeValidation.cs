using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Hotel_Reservtion_System.CastuomValidation
{
    public class RoomTypeValidation: ValidationAttribute
    {
        override protected ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            PropertyInfo? property = validationContext.ObjectType.GetProperty("roomType");
            if (property != null)
            {
                string roomType = property.GetValue(validationContext.ObjectInstance)?.ToString() ?? string.Empty;
                if (roomType.ToLower() == "single" || roomType.ToLower() == "double" || roomType.ToLower() == "suite")
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Room type must be either 'Single', 'Double', or 'Suite'.");
                }
            }
            return new ValidationResult("Room type is required.");
        }
    }
}
