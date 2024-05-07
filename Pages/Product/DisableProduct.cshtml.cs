using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Product
{
    [Authorize(Roles = "Admin, Employee")]
    public class DisableProductModel : PageModel
    {
        private ProductService _productService;
        [BindProperty]
        public Models.Product Product { get; set; }

        public DisableProductModel(ProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productService.GetProductByIdAsync(id);
            if (Product == null)
                return RedirectToPage("/NotFound");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _productService.DisableProductAsync(id);

            return RedirectToPage("/Product/GetAllProducts");
        }
    }
}
