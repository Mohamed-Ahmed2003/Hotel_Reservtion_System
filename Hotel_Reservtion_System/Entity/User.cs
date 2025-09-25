using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservtion_System.Entity
{
    public class User
    {
        [Key]
        public Guid id { get; set; }
        [StringLength(40)]
        public string? name { get; set; }
        public string? email { get; set; }
        [StringLength(40)]
        public string? password { get; set; }
        [StringLength(40)]
        public string? confirmPassword { get; set; }
        [StringLength(20)]
        public string? role { get; set; }
        [StringLength(15)]
        public string? phone { get; set; }
        public string isApproved { get; set; } = "false";
    }
}
