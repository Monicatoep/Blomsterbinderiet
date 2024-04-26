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

        public User User { get; set; }
        public OrderService OrderService { get; set; }

        public List<OrderLine> OrderLines { get; set; }
        public ProductService ProductService { get; set; }
        public CookieService CookieService { get; set; }
        public double OrderSum { get; set; }
        [BindProperty]
        public DateTime PickUpTime { get; set; }

        public CheckOutModel(UserService userService, ProductService productService, CookieService cookieService, OrderService orderService)
        {
            UserService = userService;
            this.ProductService = productService;
            this.CookieService = cookieService;
            this.OrderService = orderService;
        }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                if (userId != null)
                {
                    User = await UserService.GetUserByIdAsync(userId);
                }
            }
            IEnumerable<BasketItem> basketItems = CookieService.ReadCookieAsync(Request.Cookies).Result;
            OrderLines = CookieService.LoadOrderLinesAsync(basketItems).Result.ToList();
            OrderSum = OrderService.GetOrderSum(OrderLines);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User = await UserService.GetUserByHttpContextAsync(HttpContext);
            IEnumerable<BasketItem> basketItems = await CookieService.ReadCookieAsync(Request.Cookies);
            OrderLines = CookieService.LoadOrderLinesAsync(basketItems).Result.ToList();
            
            Models.Order order = new(User, DateTime.Now, PickUpTime);
            await OrderService.AddOrderAsync(order);
            foreach (OrderLine line in OrderLines)
            {               
               await OrderService.AddOrderLineAsync(new OrderLine(order, line.Product, line.Amount));
            }
            CookieService.SaveCookieAsync(Response.Cookies, null);
            return RedirectToPage("/Basket/Confirmation");
            
        }
    }
}
