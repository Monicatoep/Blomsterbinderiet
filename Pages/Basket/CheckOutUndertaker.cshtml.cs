using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Blomsterbinderiet.Pages.Basket
{
    public class CheckOutUndertakerModel : PageModel
    {
        public ProductService ProductService { get; set; }
        private CookieService CookieService { get; set; }
        private UserService UserService { get; set; }
        private OrderService OrderService { get; set; }
        public User User { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public double OrderSum { get; set; }
        [DisplayName("Afd�des navn")]
        [Required(ErrorMessage = "Der skal angives afd�des navn")]
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

        public CheckOutUndertakerModel(UserService userService, ProductService productService, CookieService cookieService, OrderService orderService)
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

            await OrderService.CreateNewOrderWithDeliveryAsync(User, CeremonyStart, OrderLines, new Models.Delivery(DeseasedName, Address));

            await CookieService.SaveCookieAsync(Response.Cookies, null);
            return RedirectToPage("/Basket/Confirmation");
        }
    }
}
