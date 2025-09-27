using Hotel_Reservtion_System.CastuomValidation;
using Hotel_Reservtion_System.Entity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservtion_System.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage="the name cannot be blank")]
        public string? name { get; set; }
        [Required(ErrorMessage = "the email cannot be blank")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? email { get; set; }
        [Required(ErrorMessage = "the password cannot be blank")]
        public string? password { get; set; }
        [Required(ErrorMessage = "the confirm password cannot be blank")]
        [Compare("password", ErrorMessage = "Passwords do not match")]
        public string? confirmPassword { get; set; }
        [Required(ErrorMessage = "the phone cannot be blank")]
        public string? phone { get; set; }
        [Required(ErrorMessage = "the role cannot be blank")]
        [RoleValidation()]
        public string? role { get; set; }



        public User ConvertToUser()
        {
            User user = new User();
            user.id = Guid.NewGuid();
            user.email = this.email;
            user.password = this.password;
            user.confirmPassword = this.confirmPassword;
            user.phone = this.phone;
            user.name = this.name;
            user.role = this.role;
            user.isApproved = "false";
            return user;
        }

    }
}
