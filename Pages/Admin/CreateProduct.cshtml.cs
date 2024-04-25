using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    public class CreateProductModel : PageModel
    {
        [BindProperty]
        public InputModels.UpdateProduct Product { get; set; }
        public string Confirmation { get; set; }
        public ProductService ProductService { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public CreateProductModel(ProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            ProductService = productService;
            WebHostEnvironment = webHostEnvironment;
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
            Models.Product NewProduct = new Models.Product(Product.Name,Product.Description,Product.Price, Product.Colour);
            
            if (Product.UploadedImage != null)
            {
                NewProduct.Image = ConvertToByteArray(Product.UploadedImage).Result;
            }
            //Product.Disabled = InputProduct.Disabled;
            await ProductService.AddProductAsync(NewProduct);

            Confirmation = "Tilføjet produktet";
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

    
}
