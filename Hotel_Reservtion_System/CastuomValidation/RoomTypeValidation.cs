using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Hotel_Reservtion_System.CastuomValidation
{
    public class RoomTypeValidation: ValidationAttribute
    {
        private enum RoomTypes
        {
            Single,
            Double,
            Suite
        }
        override protected ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            PropertyInfo? property = validationContext.ObjectType.GetProperty("roomType");
            if (property != null)
            {
                string roomType = property.GetValue(validationContext.ObjectInstance)?.ToString() ?? string.Empty;
                for (int i = 0; i < Enum.GetNames(typeof(RoomTypes)).Length; i++)
                {
                    if (string.Equals(roomType, Enum.GetNames(typeof(RoomTypes))[i], StringComparison.OrdinalIgnoreCase))
                    {
                        return ValidationResult.Success;
                    }
                }
                return new ValidationResult("Room type must be either 'Single', 'Double', or 'Suite'.");
                
            }
            return new ValidationResult("Room type is required.");
        }
    }
}
