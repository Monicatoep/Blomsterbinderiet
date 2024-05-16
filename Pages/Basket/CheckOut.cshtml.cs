using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.Basket
{
    public class CheckOutModel : PageModel
    {
        public ProductService ProductService { get; set; }
        private CookieService CookieService { get; set; }
        private UserService UserService { get; set; }
        private OrderService OrderService { get; set; }
        public User User { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public double OrderSum { get; set; }
        [DisplayName("Afhentningstidspunkt")]
        [Required(ErrorMessage = "Der skal angives et afhentningsstidspunkt")]
        [BindProperty]
        public DateTime PickUpDate { get; set; }
        public string? Address { get; set; }

        public CheckOutModel(UserService userService, ProductService productService, CookieService cookieService, OrderService orderService)
        {
            UserService = userService;
            this.ProductService = productService;
            this.CookieService = cookieService;
            this.OrderService = orderService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.User = await UserService.GetUserByHttpContextAsync(HttpContext);
          
            IEnumerable<BasketItem> basketItems = CookieService.ReadCookie(Request.Cookies);
            OrderLines = CookieService.LoadOrderLines(basketItems).ToList();
            OrderSum = OrderService.GetOrderSum(OrderLines);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IEnumerable<BasketItem> basketItems = CookieService.ReadCookie(Request.Cookies);
            if (!ModelState.IsValid)
            {
                User = await UserService.GetUserByHttpContextAsync(HttpContext);
                OrderLines = CookieService.LoadOrderLines(basketItems).ToList();
                return Page();
            }
            User = await UserService.GetUserByHttpContextAsync(HttpContext);
            OrderLines = CookieService.LoadOrderLines(basketItems).ToList();

            await OrderService.CreateNewOrderAsync(User, PickUpDate, OrderLines);
          
            CookieService.SaveCookie(Response.Cookies, null);
            return RedirectToPage("/Basket/Confirmation");
        }
    }
}
