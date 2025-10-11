using Hotel_Reservtion_System.Entity;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservtion_System.DTO
{
    public class BookingDTO
    {
        [Required(ErrorMessage ="User information is required")]
        public User user { get; set; }

        [Required(ErrorMessage = "checkIn information is required")]
        public DateTime checkIn { get; set; }

        [Required(ErrorMessage = "checkOut information is required")]
        public DateTime checkOut { get; set; }
        [StringLength(10)]
        public string? status { get; set; }
        [Required(ErrorMessage = "Room ID is required")]
        public Guid roomId { get; set; }
    }
}
