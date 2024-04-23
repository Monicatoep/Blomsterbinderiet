using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.Basket
{
    public class ConfirmationModel : PageModel
    {
        public IEnumerable<BasketItem> BasketItems { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public ProductService ProductService { get; set; }
        public BasketCookieService CookieService { get; set; }
        public double OrderSum { get; set; }
        public UserService UserService { get; set; }
        public OrderService OrderService { get; set; }
        public User User { get; set; }

        public ConfirmationModel(UserService userService, ProductService productService, BasketCookieService cookieService, OrderService orderService)
        {
            UserService = userService;
            this.ProductService = productService;
            this.CookieService = cookieService;
            this.OrderService = orderService;

        }
        public void OnGet()
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (userId != null)
            {
                User = UserService.GetUserByIdAsync(Convert.ToInt32(userId));
            }
            Order order = new( User, DateTime.Now);
            OrderService.AddOrderAsync(order);
            foreach(OrderLine line in OrderLines)
            {   line.Order = order;
                line.OrderID = order.Id;
                line.ProductID = line.Product.ID;
                OrderService.AddOrderLineAsync(line);
            }
                
        }
    }
}
