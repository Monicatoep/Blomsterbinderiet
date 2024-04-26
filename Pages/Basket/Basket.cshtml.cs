using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using System;

namespace Blomsterbinderiet.Pages.Basket
{
    public class BasketModel : PageModel
    {
        public ICollection<BasketItem> BasketItems { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public ProductService ProductService { get; set; }
        public CookieService CookieService { get; set; }
        public OrderService OrderService { get; set; }
        public double OrderSum { get; set; }

        public BasketModel(ProductService productService, CookieService cookieService, OrderService orderService)
        {
            this.ProductService = productService;
            this.CookieService = cookieService;
            this.OrderService = orderService;
        }

        public void OnGet()
        {
            if (CookieService.ReadCookie(Request.Cookies).Result == null)
            {
                BasketItems = null;
                OrderLines = new();
            }
            else
            {
                BasketItems = CookieService.ReadCookie(Request.Cookies).Result;
                if(BasketItems != null)
                {
                    OrderLines = CookieService.LoadOrderLines(BasketItems).Result.ToList();
                } else
                {
                    OrderLines = new();
                }
            }
            
            OrderSum = OrderService.GetOrderSum(OrderLines);
        }

        public async Task<IActionResult> OnPostPlus(int id)
        {
            IEnumerable<BasketItem> basketItems = await CookieService.PlusOne(Request.Cookies, Response.Cookies, id);

            OrderLines = CookieService.LoadOrderLines(basketItems).Result.ToList();

            OrderSum = OrderService.GetOrderSum(OrderLines);
            
            return Page();
        }

        public async Task<IActionResult> OnPostMinus(int id)
        {
            IEnumerable<BasketItem> basketItems = await CookieService.MinusOne(Request.Cookies, Response.Cookies, id);

            OrderLines = CookieService.LoadOrderLines(basketItems).Result.ToList();

            OrderSum = OrderService.GetOrderSum(OrderLines);

            return Page();
        }
    }
}
