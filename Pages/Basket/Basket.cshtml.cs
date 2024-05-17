using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Service;

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

        public BasketModel(/*ProductService productService,*/ CookieService cookieService, OrderService orderService)
        {
            //this.ProductService = productService;
            this.CookieService = cookieService;
            this.OrderService = orderService;
        }

        public IActionResult OnGet()
        {
            BasketItems = CookieService.ReadCookie(Request.Cookies);
            if(BasketItems != null)
            {
                OrderLines = CookieService.LoadOrderLines(BasketItems).ToList();
            }
            OrderSum = OrderService.GetOrderSum(OrderLines);
            return Page();
        }

        public IActionResult OnPostPlus(int id)
        {
            IEnumerable<BasketItem> basketItems = CookieService.PlusOne(Request.Cookies, Response.Cookies, id);

            OrderLines = CookieService.LoadOrderLines(basketItems).ToList();

            OrderSum = OrderService.GetOrderSum(OrderLines);
            
            return Page();
        }

        public IActionResult OnPostMinusAsync(int id)
        {
            IEnumerable<BasketItem> basketItems = CookieService.MinusOne(Request.Cookies, Response.Cookies, id);

            OrderLines = CookieService.LoadOrderLines(basketItems).ToList();

            if (OrderLines.Count == 0)
            {
                OrderLines = null;
                CookieService.SaveCookie(Response.Cookies, null);
            }

            OrderSum = OrderService.GetOrderSum(OrderLines);


            return Page();
        }

        public IActionResult OnPostRemove(int id)
        {
            IEnumerable<BasketItem> basketItems = CookieService.Remove(Request.Cookies, Response.Cookies, id);

            OrderLines = CookieService.LoadOrderLines(basketItems).ToList();

            if(OrderLines.Count == 0)
            {
                OrderLines = null;
                CookieService.SaveCookie(Response.Cookies, null);
            }

            OrderSum = OrderService.GetOrderSum(OrderLines);

            return Page();
        }
    }
}
