using Hotel_Reservtion_System.Entity;

namespace Hotel_Reservtion_System.ServicesContracts
{
    public interface IEmailServices
    {
        public Task sendEmail(string userEmail, Invoice invoice);
    }
}
