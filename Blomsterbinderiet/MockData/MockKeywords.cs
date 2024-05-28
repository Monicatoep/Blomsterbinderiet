using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.MockData
{
	public class MockKeywords
	{
		public static List<Keyword> keywords = new List<Keyword>()
		{
			new Keyword("Bryllup", new List<Product>()),
			new Keyword("Valentine", new List<Product>()),
			new Keyword("Konfirmation", new List<Product>()),
			new Keyword("Bryllup", new List<Product>()),
			new Keyword("Romantik", new List<Product>()),
			new Keyword("Bryllup", new List<Product>()),
			new Keyword("Arbejde", new List<Product>())
		};

		public static List<Keyword> GetMockKeywords()
		{
			return keywords;
		}
	}
}
