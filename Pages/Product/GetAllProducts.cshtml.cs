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

        public List<Models.Product> Products { get; private set; } = new List<Models.Product>();

		public void OnGet()
        {
            Products = ProductService.GetProductsAsync().Result.ToList();
        }
    }
}
