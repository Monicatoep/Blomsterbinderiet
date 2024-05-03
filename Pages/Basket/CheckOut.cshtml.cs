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
        public UserService UserService { get; set; }

        public User User { get; set; }
        public OrderService OrderService { get; set; }

        public List<OrderLine> OrderLines { get; set; }
        public ProductService ProductService { get; set; }
        public CookieService CookieService { get; set; }
        public double OrderSum { get; set; }

        [DisplayName("Afhentningstidspunkt")]
        [Required(ErrorMessage = "Der skal angives et afhentningsstidspunkt")]
        [BindProperty]
        public DateTime PickUpDate { get; set; }

        [DisplayName("Afdødes navn")]
        [Required(ErrorMessage = "Der skal angives afdødes navn")]
        [BindProperty]
        public string? DeseasedName { get; set; }

        [DisplayName("Begravelses start")]
        [Required(ErrorMessage = "Der skal angives begravelses start")]
        [BindProperty]
        public DateTime CeremonyStart { get; set; }

        [DisplayName("Leveringsadresse")]
        [Required(ErrorMessage = "Der skal angives en leveringsadresse")]
        [BindProperty]
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
          
            IEnumerable<BasketItem> basketItems = CookieService.ReadCookieAsync(Request.Cookies).Result;
            OrderLines = CookieService.LoadOrderLinesAsync(basketItems).Result.ToList();
            OrderSum = OrderService.GetOrderSum(OrderLines);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IEnumerable<BasketItem> basketItems = await CookieService.ReadCookieAsync(Request.Cookies);
            if (!ModelState.IsValid)
            {
                User = await UserService.GetUserByHttpContextAsync(HttpContext);
                OrderLines = CookieService.LoadOrderLinesAsync(basketItems).Result.ToList();
                return Page();
            }
            User = await UserService.GetUserByHttpContextAsync(HttpContext);
            OrderLines = CookieService.LoadOrderLinesAsync(basketItems).Result.ToList();

            await OrderService.CreateNewOrderAsync(User, PickUpDate, OrderLines);
          
            await CookieService.SaveCookieAsync(Response.Cookies, null);
            return RedirectToPage("/Basket/Confirmation");
            
        }

        public async Task<IActionResult> OnPostWithDeliveryAsync()
        {
            IEnumerable<BasketItem> basketItems = await CookieService.ReadCookieAsync(Request.Cookies);
            if (!ModelState.IsValid)
            {
                User = await UserService.GetUserByHttpContextAsync(HttpContext);
                OrderLines = CookieService.LoadOrderLinesAsync(basketItems).Result.ToList();
                return Page();
            }
            User = await UserService.GetUserByHttpContextAsync(HttpContext);
            OrderLines = CookieService.LoadOrderLinesAsync(basketItems).Result.ToList();

            await OrderService.CreateNewOrderWithDeliveryAsync(User, PickUpDate, OrderLines, new Models.Delivery(DeseasedName, CeremonyStart, Address));

            await CookieService.SaveCookieAsync(Response.Cookies, null);
            return RedirectToPage("/Basket/Confirmation");

        }
    }
}
