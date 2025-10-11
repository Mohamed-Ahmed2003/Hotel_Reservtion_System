using Hotel_Reservtion_System.Entity;

namespace Hotel_Reservtion_System.ServicesContracts
{
    public interface IInvoiceService
    {
        public Invoice MakeInvoice(Booking booking, double taxAmount,double discount, User employee);
    }
}
