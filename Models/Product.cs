﻿using System.ComponentModel;
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
		[Required(ErrorMessage = "Skal angives et navn")]
        public string Name { get; set; }

		[DisplayName("Beskrivelse")]
		[Required(ErrorMessage = "Skal angives en beskrivelse")]
        public string Description { get; set; }

		[DisplayName("Pris")]
		[Required(ErrorMessage = "Skal angives en pris")]
		[Range(1, int.MaxValue, ErrorMessage = "Pris skal koste 0kr eller mere")]
        public double Price { get; set; }

		[NotMapped]
		[DisplayName("Uploadet billede")]
		public IFormFile? UploadedImage { get; set; }

		public byte[]? Image { get; set; }

        public ICollection<Keyword> Keywords { get; set; }

		[DisplayName("Farve")]
		[Required(ErrorMessage = "Farve skal angives")]
        public string Colour { get; set; }

		[DisplayName("Deaktiveret")]
		public bool Disabled { get; set; }

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
