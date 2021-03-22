using System.ComponentModel.DataAnnotations;

namespace ActivityTrackerApi.Data.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
