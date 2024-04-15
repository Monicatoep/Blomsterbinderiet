using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.MockData;

namespace Blomsterbinderiet.Pages.Product
{
    public class GetAllProductsModel : PageModel
    {
		public List<Models.Product> Products { get; private set; } = new List<Models.Product>();

		public void OnGet()
        {
            Products = MockProducts.GetMockProducts();
        }
    }
}
