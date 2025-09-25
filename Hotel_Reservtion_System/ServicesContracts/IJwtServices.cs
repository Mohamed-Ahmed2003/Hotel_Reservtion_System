namespace Hotel_Reservtion_System.ServicesContracts
{
    public interface IJwtServices
    {
        public string? GenerateToken(string email, string role);
    }
}
