using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;

namespace Blomsterbinderiet.Pages.Basket
{
    public class BasketModel : PageModel
    {
        public IEnumerable<BasketItem> BasketItems { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public ProductService BasketService { get; set; }
        public BasketCookieService CookieService { get; set; }

        public BasketModel(ProductService service, BasketCookieService cookieService)
        {
            this.BasketService = service;
            this.CookieService = cookieService;
        }

        public void OnGet()
        {
            BasketItems = CookieService.ReadCookie(Request.Cookies);
            OrderLines = new();
            if (BasketItems != null)
            {
                foreach (BasketItem BItem in BasketItems)
                {
                    Models.Product line = BasketService.GetProductByIdAsync(BItem.ProductID).Result;
                    OrderLine Temporary = new() { Amount = BItem.Amount, Product = line };
                    OrderLines.Add(Temporary);
                }
            }
        }

        public IActionResult OnPostPlus(int ID)
        {
            Console.WriteLine("hello");
            foreach (BasketItem BItem in BasketItems)
            {
                if (ID == BItem.ProductID)
                {
                    BItem.Amount++;
                }
            }
            return Page();
        }
    }
}
