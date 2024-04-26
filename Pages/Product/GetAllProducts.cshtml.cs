using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.MockData;
using Blomsterbinderiet.Service;

namespace Blomsterbinderiet.Pages.Product
{
    public class GetAllProductsModel : PageModel
    {
        private ProductService ProductService { get; set; }

        public GetAllProductsModel(ProductService Service)
        {
            ProductService = Service;
        }

        public IEnumerable<Models.Product> Products { get; private set; }

		public async Task OnGetAsync()
        {
            Products = await ProductService.GetProductsAsync();
        }
    }
}
