using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Blomsterbinderiet.Model
{
	public class Product
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public byte[]? Image { get; set; }
        public ICollection<Keyword> Keywords { get; set; }
        public string Colour { get; set; }

		public Product()
		{
		}

		public Product(string name, string description, double price, /*byte[] image,*/ ICollection<Keyword> keywords, string colour)
		{
			Name = name;
			Description = description;
			Price = price;
			//Image = image;
			Keywords = keywords;
			Colour = colour;
		}

		public override string ToString()
		{
			return $"{{{nameof(ID)}={ID.ToString()}, {nameof(Name)}={Name}, {nameof(Description)}={Description}, {nameof(Price)}={Price.ToString()}, {nameof(Image)}={Image}, {nameof(Keywords)}={Keywords}, {nameof(Colour)}={Colour}}}";
		}
	}
}
