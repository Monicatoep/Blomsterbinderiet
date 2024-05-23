using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Models
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Der skal angives afdødes navn")]
        public string DeseasedName { get; set; }

        [Required(ErrorMessage = "Der skal angives en leveringsadresse")]
        public string Address { get; set; }

        public Delivery()
        {
        }

        public Delivery(string deseasedName, string address)
        {
            DeseasedName = deseasedName;
            Address = address;
        }

        public override string ToString()
        {
            return $"{{{nameof(ID)}={ID}, {nameof(DeseasedName)}={DeseasedName}, {nameof(Address)}={Address}}}";
        }
    }
}
