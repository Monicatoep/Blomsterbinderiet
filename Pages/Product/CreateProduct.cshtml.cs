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
        public ServiceGeneric<Models.Keyword> KeywordService { get; set; }
        public ImageService Tools { get; set; }
        public List<Models.Keyword> ProductKeywords{ get; set; }
        [BindProperty]
        public IEnumerable<Models.Keyword> AvaibleKeywords { get; set; }

        public CreateProductModel(ProductService productService, ServiceGeneric<Keyword> keywordService, ImageService tools)
        {
            ProductService = productService;
            KeywordService = keywordService;
            Tools = tools;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            AvaibleKeywords = await KeywordService.GetAllDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetAddKeywordAsync(int id)
        {
            ProductKeywords.Add(await KeywordService.GetByIdAsync(id));

            AvaibleKeywords = (await KeywordService.GetAllDataAsync()).Except(ProductKeywords);
            return Page();
        }

        public async Task<IActionResult> OnGetRemoveKeywordAsync(int id)
        {
            ProductKeywords.Remove(await KeywordService.GetByIdAsync(id));

            AvaibleKeywords = (await KeywordService.GetAllDataAsync()).Except(ProductKeywords);
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

            Confirmation = "Tilføjet produktet";
            return Page();
        }
    }


}
