using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blomsterbinderiet.Models
{
	public class Keyword
	{
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }

		public Keyword()
		{
		}

		public Keyword(string name, string description, ICollection<Product> products)
		{
			Name = name;
			Description = description;
			Products = products;
		}

		public override string ToString()
		{
			return $"{{{nameof(ID)}={ID.ToString()}, {nameof(Name)}={Name}, {nameof(Description)}={Description}, {nameof(Products)}={Products}}}";
		}
	}
}
