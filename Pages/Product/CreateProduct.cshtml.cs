using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Product
{
    [Authorize(Roles = "Admin, Employee")]
    public class CreateProductModel : PageModel
    {
        private ProductService ProductService { get; set; }
        public ImageService ImageService { get; set; }
        private KeywordService KeywordService { get; set; }
        [BindProperty]
        public Models.Product Product { get; set; }
        [BindProperty]
        public int[] KeywordIDs { get; set; }
        public string Confirmation { get; set; }
        public IEnumerable<Keyword> ProductKeywords{ get; set; }

        public CreateProductModel(ProductService productService, ImageService imageService, KeywordService keywordService)
        {
            ProductService = productService;
            ImageService = imageService;
            KeywordService = keywordService;
        }



        public async Task<IActionResult> OnGetAsync()
        {
            ProductKeywords = await KeywordService.GetAllKeywordsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    Confirmation = "Oprettelse fejlede";
            //    return Page();
            //}
            //Product.Keywords = new List<Keyword>();
            //Keyword keyword;
            foreach (int id in KeywordIDs)
            {
                //keyword = await KeywordService.GetKeywordByIdAsync(id);
                //keyword.Name = null;
                //Product.Keywords.Add(keyword);
                Console.WriteLine(id);
            }
            //maybe a solution https://stackoverflow.com/questions/4253165/insert-update-many-to-many-entity-framework-how-do-i-do-it
            //Product.Keywords = null;
            ProductKeywords = await KeywordService.GetAllKeywordsAsync();
            if(Product.UploadedImage != null)
            {
                Product.Image = ImageService.ConvertToByteArray(Product.UploadedImage);
            }
            await ProductService.AddProductAsync(Product, KeywordIDs);

            Confirmation = "Tilføjet produktet";
            return Page();
        }
    }
}
