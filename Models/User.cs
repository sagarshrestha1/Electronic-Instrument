using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = "";


    }
}
