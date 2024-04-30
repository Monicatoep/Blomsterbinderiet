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
        public DateOnly PickUpDate { get; set; }
        [BindProperty]
        public TimeOnly PickUpTime { get; set; }

        [BindProperty]
        public Models.Delivery? Delivery { get; set; }

        public CheckOutModel(UserService userService, ProductService productService, CookieService cookieService, OrderService orderService)
        {
            UserService = userService;
            this.ProductService = productService;
            this.CookieService = cookieService;
            this.OrderService = orderService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            User = await UserService.GetUserByHttpContextAsync(HttpContext);
          
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

            await OrderService.CreateNewOrder(User, PickUpDate, PickUpTime, OrderLines);
          
            await CookieService.SaveCookieAsync(Response.Cookies, null);
            return RedirectToPage("/Basket/Confirmation");
            
        }
    }
}
