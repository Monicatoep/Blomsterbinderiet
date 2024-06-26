using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Pages.Basket
{
    [Authorize(Roles = "Customer")]
    public class CheckOutModel : PageModel
    {
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

        public string Message { get; set; }

        public CheckOutModel(UserService userService, CookieService cookieService, OrderService orderService)
        {
            UserService = userService;
            CookieService = cookieService;
            OrderService = orderService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            User = await UserService.GetUserByHttpContextAsync(HttpContext);
          
            IEnumerable<BasketItem> basketItems = CookieService.ReadCookie(Request.Cookies);
            OrderLines = await CookieService.LoadOrderLinesAsync(basketItems);
            OrderSum = OrderService.GetOrderSum(OrderLines);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IEnumerable<BasketItem> basketItems = CookieService.ReadCookie(Request.Cookies);
            if (!ModelState.IsValid)
            {
                User = await UserService.GetUserByHttpContextAsync(HttpContext);
                OrderLines = await CookieService.LoadOrderLinesAsync(basketItems);
                return Page();
            }

            User = await UserService.GetUserByHttpContextAsync(HttpContext);
            OrderLines = await CookieService.LoadOrderLinesAsync(basketItems);

            if (!(PickUpDate >= DateTime.Now.AddDays(1)))
            {
                Message = "Du skal v�lge et afhentningstidspunkt der er minimum 24 timer fra nu.";
                return Page();
            }
            else
            {
                await OrderService.CreateNewOrderAsync(User, PickUpDate, OrderLines);

                CookieService.SaveCookie(Response.Cookies, null);
                return RedirectToPage("/Basket/Confirmation");
            }
        }
    }
}
