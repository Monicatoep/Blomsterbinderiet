using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Blomsterbinderiet.Service;
using Blomsterbinderiet.Models;
using System.Text.Json;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

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
            await CookieService.PlusManyAsync(Request.Cookies, Response.Cookies, ProductID, Amount);

            Product = ProductService.GetProductByIdAsync(ProductID).Result;
            return Page();
        }
    }
}
