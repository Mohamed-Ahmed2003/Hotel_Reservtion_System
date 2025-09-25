using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Hotel_Reservtion_System.DTO
{
    public class AuthenticationResponse
    {
        public string? token { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? role { get; set; }
    }
}
