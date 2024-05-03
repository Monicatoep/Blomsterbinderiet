using Blomsterbinderiet.Models;
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
        public ImageService Tools { get; set; }
        public List<Models.Keyword> ProductKeywords{ get; set; }
        [BindProperty]
        public IEnumerable<Models.Keyword> AvaibleKeywords { get; set; }

        public CreateProductModel(ProductService productService, ImageService tools)
        {
            ProductService = productService;
            Tools = tools;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnGetAddKeywordAsync(int id)
        {
            return Page();
        }

        public async Task<IActionResult> OnGetRemoveKeywordAsync(int id)
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Confirmation = "Oprettelse fejlede";
                return Page();
            }

            await ProductService.AddProductAsync(Product.UpdateParameterWithNewValues(new()));

            Confirmation = "Tilf�jet produktet";
            return Page();
        }
    }


}
