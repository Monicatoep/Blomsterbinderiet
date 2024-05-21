using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Service;

namespace Blomsterbinderiet.Pages.Basket
{
    public class BasketModel : PageModel
    {
        public CookieService CookieService { get; set; }
        private OrderService OrderService { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public double OrderSum { get; set; }

        public BasketModel(CookieService cookieService, OrderService orderService)
        {
            this.CookieService = cookieService;
            this.OrderService = orderService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            BasketItems = CookieService.ReadCookie(Request.Cookies);
            if(BasketItems != null)
            {
                OrderLines = await CookieService.LoadOrderLinesAsync(BasketItems);
            }
            OrderSum = OrderService.GetOrderSum(OrderLines);
            return Page();
        }

        public async Task<IActionResult> OnPostPlusAsync(int id)
        {
            IEnumerable<BasketItem> basketItems = CookieService.PlusOne(Request.Cookies, Response.Cookies, id);

            OrderLines = await CookieService.LoadOrderLinesAsync(basketItems);

            OrderSum = OrderService.GetOrderSum(OrderLines);
            
            return Page();
        }

        public async Task<IActionResult> OnPostMinusAsync(int id)
        {
            IEnumerable<BasketItem> basketItems = CookieService.MinusOne(Request.Cookies, Response.Cookies, id);

            OrderLines = await CookieService.LoadOrderLinesAsync(basketItems);

            if (OrderLines.Count == 0)
            {
                OrderLines = null;
                CookieService.SaveCookie(Response.Cookies, null);
            }

            OrderSum = OrderService.GetOrderSum(OrderLines);


            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int id)
        {
            IEnumerable<BasketItem> basketItems = CookieService.RemoveBasketItem(Request.Cookies, Response.Cookies, id);

            OrderLines = await CookieService.LoadOrderLinesAsync(basketItems);

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
