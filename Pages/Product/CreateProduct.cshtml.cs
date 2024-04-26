using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Product
{
    public class CreateProductModel : PageModel
    {
        [BindProperty]
        public InputModels.UpdateProduct Product { get; set; }
        public string Confirmation { get; set; }
        public ProductService ProductService { get; set; }
        public Tools Tools { get; set; }

        public CreateProductModel(ProductService productService, Tools tools)
        {
            ProductService = productService;
            Tools = tools;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Confirmation = "Oprettelse fejlede";
                return Page();
            }

            await ProductService.AddProductAsync(Product.UpdateParameterWithNewValues(new()));

            Confirmation = "Tilføjet produktet";
            return Page();
        }
    }


}
