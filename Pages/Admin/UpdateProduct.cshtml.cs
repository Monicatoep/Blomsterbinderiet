using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Blomsterbinderiet.Migrations;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Pages.Admin
{
    public class UpdateProductModel : PageModel
    {
        [BindProperty]
        public InputModels.UpdateProduct InputProduct { get; set; }
        public string Confirmation { get; set; }
        public ProductService ProductService { get; set; }
        public Tools tools { get; set; }

        public UpdateProductModel(ProductService productService, Tools tools)
        {
            ProductService = productService;
            this.tools = tools;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Models.Product Product = await ProductService.GetProductByIdAsync(id);
            InputProduct = new() { ID = Product.ID, Description = Product.Description, Name = Product.Name, Price = Product.Price, /*Disabled=Product.Disabled*/ };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                Confirmation = "Opdatering fejlede";
                return Page();
            }

            Models.Product Product = ProductService.GetProductByIdAsync(InputProduct.ID).Result;

            Models.Product temp = new Models.Product(Product, InputProduct,tools);

            ProductService.UpdateProduct(temp);

            Confirmation = "Opdaterede produktet";
            return Page();
        }

        
    }
}
