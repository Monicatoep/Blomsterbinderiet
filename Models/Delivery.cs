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

        [Required(ErrorMessage = "Der skal angives begravelses start")]
        public DateTime CeremonyStart { get; set; }

        [Required(ErrorMessage = "Der skal angives en leveringsadresse")]
        public string Address { get; set; }

        public Delivery(string deseasedName, DateTime ceremonyStart, string address)
        {
            DeseasedName = deseasedName;
            CeremonyStart = ceremonyStart;
            Address = address;
        }

        public override string ToString()
        {
            return $"{{{nameof(ID)}={ID.ToString()}, {nameof(DeseasedName)}={DeseasedName}, {nameof(CeremonyStart)}={CeremonyStart.ToString()}, {nameof(Address)}={Address}}}";
        }
    }
}
