using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Service;
using System.ComponentModel;

namespace Blomsterbinderiet.Pages.Product
{
    public class ProductDetailsModel : PageModel
    {
        private ProductService ProductService { get; set; }
        private CookieService CookieService { get; set; }
        [BindProperty]
        [DisplayName("Mængde")]
        public int Amount { get; set; }
        [BindProperty]
        public int ProductID { get; set; }
        public Models.Product Product { get; set; }
        public string Message { get; set; }

        public ProductDetailsModel(ProductService service, CookieService cookieService)
        {
            this.ProductService = service;
            this.CookieService = cookieService;
        }

        public async Task OnGetAsync(int id)
        {
            Product = await ProductService.GetProductByIdAsync(id);
        }

        //cookies can only store string values
        public async Task<IActionResult> OnPostAsync()
        {
            CookieService.PlusMany(Request.Cookies, Response.Cookies, ProductID, Amount);

            Product = await ProductService.GetProductByIdAsync(ProductID);
            Message = $"Tilføjede {Amount} produkt til kurven";
            return Page();
        }
    }
}
