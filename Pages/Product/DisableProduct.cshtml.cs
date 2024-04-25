using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Product
{
    public class DisableProductModel : PageModel
    {
        private ProductService _productService;

        public DisableProductModel(ProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Models.Product Product { get; set; }

        public IActionResult OnGet(int id)
        {
            Product = _productService.GetProductByIdAsync(id).Result;
            if (Product == null)
                return RedirectToPage("/NotFound");

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            Product = _productService.GetProductByIdAsync(id).Result;
            Product.Disabled = true;
            _productService.UpdateProduct(Product);

            return RedirectToPage("/Product/GetAllProducts");
        }
    }
}
