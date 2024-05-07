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
        public InputModels.UpdateProduct InputProduct { get; set; }
        public string Confirmation { get; set; }

        public UpdateProductModel(ProductService productService, ImageService tools)
        {
            ProductService = productService;
            this.ImageService = tools;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Models.Product Product = await ProductService.GetProductByIdAsync(id);
            InputProduct = new(Product);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Confirmation = "Opdatering fejlede";
                return Page();
            }

            Models.Product Product = await ProductService.GetProductByIdAsync(InputProduct.ID);

            ProductService.UpdateProductAsync(InputProduct.UpdateParameterWithNewValues(Product));

            Confirmation = "Opdaterede produktet";
            return Page();
        }
    }
}
