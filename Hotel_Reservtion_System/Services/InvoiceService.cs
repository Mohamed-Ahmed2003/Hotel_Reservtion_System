using Hotel_Reservtion_System.DTO;
using Hotel_Reservtion_System.Entity;
using Hotel_Reservtion_System.ServicesContracts;

namespace Hotel_Reservtion_System.Services
{
    public class InvoiceService : IInvoiceService
    {
        public Invoice MakeInvoice(Booking booking, double taxAmount, double discount,User employee)
        {
            TimeSpan stayDuration = (TimeSpan)(booking.checkOut - booking.checkIn);
            int numberOfDays = stayDuration.Days;

            Invoice newInvoice = new Invoice
            {
                id = Guid.NewGuid(),
                booking = booking,
                totalAmount = booking.room.price * numberOfDays,
                tax = (booking.room.price * numberOfDays) * taxAmount,
                discount = (booking.room.price * numberOfDays) * discount,
                finalAmount = (booking.room.price * numberOfDays) + ((booking.room.price * numberOfDays) * 0.1),
                status = "paid",
                createdBy = employee
            };

            return newInvoice;
        }
    }
}
