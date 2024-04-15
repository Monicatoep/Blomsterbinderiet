using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Models
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]    
        public string Email { get; set; }

        public string? Role { get; set; }

        [Required]
        public string Phone { get; set; }

        [StringLength(maximumLength:255)]
        public string? Address { get; set; }

        public User(string password, string role, string email, string phone, string address)
        {
            Password = password;
            Role = role;
            Email = email;
            Phone = phone;
            Address = address;
        }

        public User()
        {
        }
        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Password)}={Password}, {nameof(Email)}={Email}, {nameof(Role)}={Role}, {nameof(Phone)}={Phone}, {nameof(Address)}={Address}}}";
        }
    }
}
