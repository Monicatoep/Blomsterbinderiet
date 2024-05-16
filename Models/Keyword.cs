using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blomsterbinderiet.Models
{
	public class Keyword
	{
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
        [Required(ErrorMessage = "Du skal indtaste et navn")]
        public string Name { get; set; }
        public ICollection<Product>? Products { get; set; }

		public Keyword()
		{
		}

        public Keyword(string name)
        {
            Name = name;
        }

        public Keyword(string name, ICollection<Product> products)
		{
			Name = name;
			Products = products;
		}

		public override string ToString()
		{
			return $"{{{nameof(ID)}={ID}, {nameof(Name)}={Name}, {nameof(Products)}={Products}}}";
		}
	}
}
