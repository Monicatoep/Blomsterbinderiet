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
        private ProductService ProductService { get; set; }
        public CookieService CookieService { get; set; }
        private OrderService OrderService { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public double OrderSum { get; set; }

        public BasketModel(ProductService productService, CookieService cookieService, OrderService orderService)
        {
            this.ProductService = productService;
            this.CookieService = cookieService;
            this.OrderService = orderService;
        }

        public async Task OnGetAsync()
        {
            if (CookieService.ReadCookie(Request.Cookies) == null)
            {
                BasketItems = null;
                OrderLines = new List<OrderLine>();
            }
            else
            {
                BasketItems = CookieService.ReadCookie(Request.Cookies);
                if(BasketItems != null)
                {
                    OrderLines = CookieService.LoadOrderLines(BasketItems).ToList();
                } else
                {
                    OrderLines = new List<OrderLine>();
                }
            }
            
            OrderSum = OrderService.GetOrderSum(OrderLines);
        }

        public async Task<IActionResult> OnPostPlusAsync(int id)
        {
            IEnumerable<BasketItem> basketItems = CookieService.PlusOne(Request.Cookies, Response.Cookies, id);

            OrderLines = CookieService.LoadOrderLines(basketItems).ToList();

            OrderSum = OrderService.GetOrderSum(OrderLines);
            
            return Page();
        }

        public async Task<IActionResult> OnPostMinusAsync(int id)
        {
            IEnumerable<BasketItem> basketItems = CookieService.MinusOne(Request.Cookies, Response.Cookies, id);

            OrderLines = CookieService.LoadOrderLines(basketItems).ToList();

            if (OrderLines.Count() == 0)
            {
                CookieService.SaveCookieAsync(Response.Cookies, null);
            }

            OrderSum = OrderService.GetOrderSum(OrderLines);


            return Page();
        }

        public IActionResult OnPostRemove(int id)
        {
            IEnumerable<BasketItem> basketItems = CookieService.Remove(Request.Cookies, Response.Cookies, id);

            OrderLines = CookieService.LoadOrderLines(basketItems).ToList();

            if(OrderLines.Count() == 0)
            {
                CookieService.SaveCookieAsync(Response.Cookies, null);
            }

            OrderSum = OrderService.GetOrderSum(OrderLines);

            return Page();
        }
    }
}
