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
        public ProductService ProductService { get; set; }
        public BasketCookieService CookieService { get; set; }
        public OrderService OrderService { get; set; }
        public double OrderSum { get; set; }

        public BasketModel(ProductService productService, BasketCookieService cookieService, OrderService orderService)
        {
            this.ProductService = productService;
            this.CookieService = cookieService;
            this.OrderService = orderService;
        }

        public void OnGet()
        {
            BasketItems = CookieService.ReadCookie(Request.Cookies);
            OrderLines = CookieService.LoadOrderLines(Request.Cookies).ToList();
            OrderSum = OrderService.GetOrderSum(OrderLines);
        }

        public IActionResult OnPostPlus(int id)
        {
            BasketItems = CookieService.ReadCookie(Request.Cookies);
            if (BasketItems != null)
            {
                foreach (BasketItem BItem in BasketItems)
                {
                    if (BItem.ProductID == id)
                    {
                        BItem.Amount++;
                    }
                }
            }
            CookieService.SaveCookie(Response.Cookies, BasketItems);
            OrderLines = CookieService.LoadOrderLines(Request.Cookies).ToList();
            OrderSum = OrderService.GetOrderSum(OrderLines);
            return Page();
        }
    }
}
