using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Blomsterbinderiet.Migrations;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Blomsterbinderiet.Pages.Product
{
    [Authorize(Roles = "Admin, Employee")]
    public class UpdateProductModel : PageModel
    {
        private ProductService ProductService { get; set; }
        public ImageService ImageService { get; set; }
        private KeywordService KeywordService { get; set; }
        [BindProperty]
        public Models.Product Product { get; set; }
        [BindProperty]
        public ICollection<int> KeywordIDs { get; set; }
        public string Confirmation { get; set; }
        public IEnumerable<Keyword> ProductKeywords { get; set; }

        public UpdateProductModel(ProductService productService, ImageService imageService, KeywordService keywordService)
        {
            ProductService = productService;
            ImageService = imageService;
            KeywordService = keywordService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await ProductService.GetProductIncludingKeywordsByID(id);
            KeywordIDs = new List<int>();
            foreach(Keyword keyword in Product.Keywords)
            {
                KeywordIDs.Add(keyword.ID);
            }
            ProductKeywords = await KeywordService.GetAllKeywordsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ProductKeywords = await KeywordService.GetAllKeywordsAsync();
            //if (!ModelState.IsValid)
            //{
            //    Confirmation = "Opdatering fejlede";
            //    return Page();
            //}

            if (Product.UploadedImage != null)
            {
                Product.Image = ImageService.ConvertToByteArray(Product.UploadedImage);
            }
            //await ProductService.AddProductAsync(Product, KeywordIDs);
            await ProductService.UpdateProductAsync(Product, KeywordIDs);

            Confirmation = "Opdaterede produktet";
            return Page();
        }
    }
}
