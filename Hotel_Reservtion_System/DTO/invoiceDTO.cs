using Hotel_Reservtion_System.Entity;

namespace Hotel_Reservtion_System.DTO
{
    public class invoiceDTO
    {
        public Booking? booking { get; set; }
        public double? amount { get; set; }
        public double? discount { get; set; } = 0;

        public string? status { get; set; }
        public string? createdBy { get; set; }
    }
}
