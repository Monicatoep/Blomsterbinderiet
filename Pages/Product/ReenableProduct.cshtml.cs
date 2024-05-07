using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Product
{
    [Authorize(Roles = "Admin, Employee")]
    public class ReenableProductModel : PageModel
    {
		private ProductService ProductService { get; set; }
        [BindProperty]
        public Models.Product Product { get; set; }

        public ReenableProductModel(ProductService productService)
		{
			ProductService = productService;
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			Product = await ProductService.GetProductByIdAsync(id);
			if (Product == null)
				return RedirectToPage("/NotFound");

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			await ProductService.ReenableProductAsync(id);

			return RedirectToPage("/Product/GetAllProducts");
		}
	}
}
