using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Blomsterbinderiet.Service;
using Blomsterbinderiet.Models;
using System.Text.Json;

namespace Blomsterbinderiet.Pages.Product
{
    public class ProductDetailsModel : PageModel
    {
        [BindProperty]
        public int Amount { get; set; }
        [BindProperty]
        public int ProductID { get; set; }

        private ProductService ProductService { get; set; }
        public CookieService CookieService { get; set; }
        public Models.Product Product { get; set; }

        public ProductDetailsModel(ProductService service, CookieService cookieService)
        {
            this.ProductService = service;
            this.CookieService = cookieService;
        }

        public void OnGet(int id)
        {
            Product = ProductService.GetProductByIdAsync(id).Result;
        }

        //cookies can only store string values
        public async Task<IActionResult> OnPost()
        {
            await CookieService.PlusMany(Request.Cookies, Response.Cookies, ProductID, Amount);

            Product = ProductService.GetProductByIdAsync(ProductID).Result;
            return Page();
        }
    }
}
