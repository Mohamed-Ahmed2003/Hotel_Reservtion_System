using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservtion_System.Entity
{
    public class Booking
    {
        [Key]
        public Guid id { get; set; }
        public Room? room { get; set; }
        public User? user { get; set; }
        public DateTime? checkIn { get; set; }
        public DateTime? checkOut { get; set; }
        [StringLength(10)]
        public string? status { get; set; }
    }
}
