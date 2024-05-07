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
        public ProductService ProductService { get; set; }
        public ImageService ImageService { get; set; }
        [BindProperty]
        public Models.Product Product { get; set; }
        public string Confirmation { get; set; }

        public UpdateProductModel(ProductService productService, ImageService imageService)
        {
            ProductService = productService;
            this.ImageService = imageService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await ProductService.GetProductByIdAsync(id);
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Confirmation = "Opdatering fejlede";
                return Page();
            }

            await ProductService.UpdateProductAsync(Product);

            Confirmation = "Opdaterede produktet";
            return Page();
        }
    }
}
