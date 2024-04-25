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
        public InputUpdateProductModel InputProduct { get; set; }
        public string Confirmation { get; set; }
        public ProductService ProductService { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public UpdateProductModel(ProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            ProductService = productService;
            WebHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Models.Product Product = await ProductService.GetProductByIdAsync(id);
            InputProduct = new() { ID = Product.ID, Description = Product.Description, Name = Product.Name, Price = Product.Price, Disabled=Product.Disabled };
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
            Product.Name = InputProduct.Name;
            Product.Price = InputProduct.Price;
            Product.Description = InputProduct.Description;
            if(InputProduct.UploadedImage != null)
            {
                Product.Image = ConvertToByteArray(InputProduct.UploadedImage).Result;
            }
            Product.Disabled = InputProduct.Disabled;
            ProductService.UpdateProduct(Product);

            Confirmation = "Opdaterede produktet";
            return Page();
            //return RedirectToPage("/Product/GetAllProducts");
        }

        public async Task<byte[]> ConvertToByteArray(IFormFile temp)
        {
            var filePath = Path.Combine(Path.Combine(WebHostEnvironment.WebRootPath, "images"), temp.FileName);

            using (FileStream fs = System.IO.File.Create(filePath))
            {
                temp.CopyTo(fs);
            }
            byte[] temp2 = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return temp2;
        }
    }

    [ModelMetadataType(typeof(Models.Product))]
    public class InputUpdateProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile? UploadedImage { get; set; }
        public bool Disabled { get; set; }
    }
}
