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
        public BasketCookieService CookieService { get; set; }
        public Models.Product Product { get; set; }

        public ProductDetailsModel(ProductService service, BasketCookieService cookieService)
        {
            this.ProductService = service;
            this.CookieService = cookieService;
        }

        public void OnGet(int id)
        {
            Product = ProductService.GetProductByIdAsync(id).Result;
        }

        //cookies can only store string values
        public IActionResult OnPost()
        {
            ICollection<BasketItem> temp = CookieService.ReadCookie(Request.Cookies);
            
            if (temp == null)
            {
                //Console.WriteLine("Cookie var tom");
                temp = new List<BasketItem>();
                temp.Add(new() { ProductID = ProductID, Amount = Amount });
            } else
            {
                //Console.WriteLine("Cookie var ikke tom");
                foreach(var i in temp)
                {
                    if(i.ProductID == ProductID)
                    {
                        i.Amount += Amount;
                        goto Found;
                    }
                }
                temp.Add(new() { ProductID = ProductID, Amount = Amount });
            }
        Found:
            CookieService.SaveCookie(Response.Cookies, temp);

            //the below solution is more flexible than the commented solution
            Product = ProductService.GetProductByIdAsync(ProductID).Result;
            return Page();
            //return RedirectToPage("/Product/ProductDetails", new { id = _ProductID });
            
        }
    }
}
