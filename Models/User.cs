using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Models
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]    
        public string Email { get; set; }

        public string? Role { get; set; }

        [Required]
        public string Phone { get; set; }

        [StringLength(maximumLength:255)]
        public string? Address { get; set; }

        public User(string name, string password, string role, string email, string phone, string address)
        {
            Name = name;
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
            return $"{{{nameof(ID)}={ID.ToString()}, {nameof(Name)}={Name}, {nameof(Password)}={Password}, {nameof(Email)}={Email}, {nameof(Role)}={Role}, {nameof(Phone)}={Phone}, {nameof(Address)}={Address}}}";
        }
    }
}
