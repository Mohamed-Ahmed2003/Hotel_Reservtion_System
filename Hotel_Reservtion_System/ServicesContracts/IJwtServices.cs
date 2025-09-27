using Hotel_Reservtion_System.DTO;
using Hotel_Reservtion_System.Entity;

namespace Hotel_Reservtion_System.ServicesContracts
{
    public interface IJwtServices
    {
        public string? GenerateToken(User user);
    }
}
