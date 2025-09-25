using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservtion_System.Entity
{
    public class Branch
    {
        [Key]
        public Guid id { get; set; }
        [StringLength(40)]
        public string? name { get; set; }
        [StringLength(40)]
        public string? location { get; set; }
        [StringLength(15)]
        public string? phone { get; set; }
    }
}
