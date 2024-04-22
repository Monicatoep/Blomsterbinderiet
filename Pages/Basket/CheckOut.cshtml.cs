using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.Basket
{
    public class CheckOutModel : PageModel
    { 
        public UserService UserService { get; set; }

    public Models.User User { get; set; }
        public IEnumerable<BasketItem> BasketItems { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public ProductService ProductService { get; set; }
        public BasketCookieService CookieService { get; set; }
        public double OrderSum { get; set; }

        public CheckOutModel(UserService userService, ProductService productService, BasketCookieService cookieService)
        {
            UserService = userService;
            this.ProductService = productService;
            this.CookieService = cookieService;
        }

        public void OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                if (userId != null)
                {
                    User = UserService.GetUserByIdAsync(Convert.ToInt32(userId));
                }
            }
            BasketItems = CookieService.ReadCookie(Request.Cookies);
            OrderLines = new();
            if (BasketItems != null)
            {
                foreach (BasketItem BItem in BasketItems)
                {
                    Models.Product line = ProductService.GetProductByIdAsync(BItem.ProductID).Result;
                    OrderLine Temporary = new() { Amount = BItem.Amount, Product = line };
                    OrderLines.Add(Temporary);
                    OrderSum += (Temporary.Product.Price * Temporary.Amount);
                }
            }
        }
    }
}
