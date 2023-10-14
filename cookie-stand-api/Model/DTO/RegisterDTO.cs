using System.ComponentModel.DataAnnotations;

namespace cookie_stand_api.Model.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set;}

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }


    }
}
