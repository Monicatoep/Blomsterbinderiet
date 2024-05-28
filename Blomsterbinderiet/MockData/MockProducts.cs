using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.MockData
{
	public class MockProducts
	{
        private readonly static List<Product> products = new()
		{
			new("Den Helt Rigtige", "Glæd modtageren med denne smukke buket, bundet i traditionel stil og i lyserøde nuancer.", 319, /*new List<Keyword>(),*/ "Multifarvet"),
			new("Den Kære", "Glæd ham eller hende med denne smukke og klassiske buket i lyserøde og gule nuancer.", 339, /*new List<Keyword>(),*/ "Multifarvet"),
			new("Den Yndige", "Send en hilsen med denne yndige buket i smukke lyserøde nuancer, som er bundet mellem højt og løst.", 329, /*new List<Keyword>(),*/ "Multifarvet"),
			new("Den til fødselsdagen", "Lykønsk modtageren med denne klassiske buket til fødselsdagen i røde og hvide nuancer.", 379, /*new List<Keyword>(),*/ "Multifarvet"),
			new("Den Farvrige", "Denne smukke buket er bundet i farverige nuancer med et tæt og elegant udtryk.", 379, /*new List<Keyword>(),*/ "Multifarvet"),
			new("Den Venlige", "Denne smukke buket er bundet i farverige nuancer med et tæt og elegant udtryk.", 299, /*new List<Keyword>(),*/ "Multifarvet"),
			new("Name", "description", 999, /*new List<Keyword>(), */"Multifarvet"),
			new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
            new("Name", "description", 999, /*new List<Keyword>(),*/ "Multifarvet"),
        };

		public static List<Product> GetMockProducts()
		{
			return products;
		}
	}
}
