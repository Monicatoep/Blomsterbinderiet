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
        public BasketService BasketService { get; set; }
        public BasketCookieService CookieService { get; set; }

        public BasketModel(BasketService service, BasketCookieService cookieService)
        {
            this.BasketService = service;
            this.CookieService = cookieService;
        }

        public void OnGet()
        {
            BasketItems = CookieService.ReadCookie(Request.Cookies);
            Console.WriteLine("is basketitems empty" + BasketItems == null);
            //Console.WriteLine(cookieValue);
            OrderLines = new();
            if (BasketItems != null)
            {
                foreach (BasketItem helloworld in BasketItems)
                {
                    Models.Product idk = BasketService.GetProductByIdAsync(helloworld.ProductID).Result;
                    OrderLine Temporary = new() { Amount = helloworld.Amount, Product = idk };
                    OrderLines.Add(Temporary);
                }
            }
        }
    }
}
