using Blomsterbinderiet.Model;

namespace Blomsterbinderiet.MockData
{
	public class MockKeywords
	{
		public static List<Keyword> keywords = new List<Keyword>()
		{
			new Keyword("Bryllup", "Perfekt til bryllup", new List<Product>()),
			new Keyword("Valentine", "Perfekt til valentine", new List<Product>()),
			new Keyword("Konfirmation", "Perfekt til konfirmation", new List<Product>()),
			new Keyword("Bryllup", "Perfekt til bryllup", new List<Product>()),
			new Keyword("Romantik", "Perfekt til romantikken", new List<Product>()),
			new Keyword("Bryllup", "Perfekt til bryllup", new List<Product>()),
			new Keyword("Arbejde", "Perfekt til arbejdet", new List<Product>())
		};

		public static List<Keyword> GetMockKeywords()
		{
			return keywords;
		}
	}
}
