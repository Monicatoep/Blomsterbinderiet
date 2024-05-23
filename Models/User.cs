using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Du skal indtaste et navn")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Du skal indtaste et password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Du skal indtaste en e-mailadresse")]
        public string Email { get; set; }
        
        public string? Role { get; set; }
        
        [Required(ErrorMessage = "Du skal indtaste et telefonnummer")]
        [MinLength(8, ErrorMessage = "Telefonnummer skal minimum være 8 tegn")]
        [MaxLength(12, ErrorMessage = "Telefonnummer skal maksimalt være 12 tegn")]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "Du skal indtaste en adresse")]
        [StringLength(maximumLength:255)]
        public string Address { get; set; }
        
        public string? State { get; set; }

        public User()
        {
        }

        public User(string name, string password, string role, string email, string phone, string address)
        {
            Name = name;
            Password = password;
            Role = role;
            Email = email;
            Phone = phone;
            Address = address;
            State = "Aktiv";
        }

        public override string ToString()
        {
            return $"{{{nameof(ID)}={ID}, {nameof(Name)}={Name}, {nameof(Password)}={Password}, {nameof(Email)}={Email}, {nameof(Role)}={Role}, {nameof(Phone)}={Phone}, {nameof(Address)}={Address}, {nameof(State)}={State}}}";
        }
    }
}
