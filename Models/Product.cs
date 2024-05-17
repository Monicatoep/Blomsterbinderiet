using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Blomsterbinderiet.Models
{
	public class Product
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

		[DisplayName("Navn")]
		[Required(ErrorMessage = "Der skal angives et navn")]
        public string Name { get; set; }

		[DisplayName("Beskrivelse")]
		[Required(ErrorMessage = "Der skal angives en beskrivelse")]
        public string Description { get; set; }
		
		[DisplayName("Pris")]
		[Required(ErrorMessage = "Der skal angives en pris")]
		[Range(1, double.MaxValue, ErrorMessage = "Pris skal være 0kr eller mere")]
        public double Price { get; set; }
		
		[NotMapped]
		[DisplayName("Upload billede")]
		public IFormFile? UploadedImage { get; set; }
		
		public byte[]? Image { get; set; }
        
		public ICollection<Keyword> Keywords { get; set; }
		
		[DisplayName("Farve")]
		[Required(ErrorMessage = "Der skal angives en farve")]
        public string Colour { get; set; }
		
		[DisplayName("Deaktiveret")]
		public bool Disabled { get; set; }

		public Product()
		{
		}

		public Product(string name, string description, double price, string colour)
		{
			Name = name;
			Description = description;
			Price = price;
			Colour = colour;
		}

		public override string ToString()
		{
			return $"{{{nameof(ID)}={ID.ToString()}, {nameof(Name)}={Name}, {nameof(Description)}={Description}, {nameof(Price)}={Price.ToString()}, {nameof(Image)}={Image}, {nameof(Keywords)}={Keywords}, {nameof(Colour)}={Colour}}}";
		}
	}
}
