using Blomsterbinderiet.Models;
using System.Data;
using System.Reflection.Metadata;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Blomsterbinderiet.MockData
{
	public class MockProducts
	{
		public static List<Product> products = new List<Product>()
		{
			new Product("Den Helt Rigtige", "Glæd modtageren med denne smukke buket, bundet i traditionel stil og i lyserøde nuancer.", 319, new List<Keyword>(), "Multifarvet"),
			new Product("Den Kære", "Glæd ham eller hende med denne smukke og klassiske buket i lyserøde og gule nuancer.", 339, new List<Keyword>(), "Multifarvet"),
			new Product("Den Yndige", "Send en hilsen med denne yndige buket i smukke lyserøde nuancer, som er bundet mellem højt og løst.", 329, new List<Keyword>(), "Multifarvet"),
			new Product("Den til fødselsdagen", "Lykønsk modtageren med denne klassiske buket til fødselsdagen i røde og hvide nuancer.", 379, new List<Keyword>(), "Multifarvet"),
			new Product("Den Farvrige", "Denne smukke buket er bundet i farverige nuancer med et tæt og elegant udtryk.", 379, new List<Keyword>(), "Multifarvet"),
			new Product("Den Venlige", "Denne smukke buket er bundet i farverige nuancer med et tæt og elegant udtryk.", 299, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
			new Product("Name", "description", 999, new List<Keyword>(), "Multifarvet"),
		};

		public static List<Product> GetMockProducts()
		{
			return products;
		}
	}
}
