using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Model;
using Blomsterbinderiet.MockData;

namespace Blomsterbinderiet.Pages.Product
{
    public class GetAllProductsModel : PageModel
    {
		public List<Model.Product> Products { get; private set; } = new List<Model.Product>();

		public void OnGet()
        {
            Products = MockProducts.GetMockProducts();
        }
    }
}
