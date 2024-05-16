using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.UserPages
{
    public class OrderDetailsModel : PageModel
    {
        public OrderService OrderService { get; set; }
        private UserService UserService { get; set; }
        public ProductService ProductService { get; set; }
        public Order Order { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public User User { get; set; }
        public double OrderSum { get; set; }

        public OrderDetailsModel(OrderService orderService, UserService userService, ProductService productService)
        {
            OrderService = orderService;
            UserService = userService;
            ProductService = productService;
            OrderLines = new List<OrderLine>();
            
        }

        public async Task OnGetAsync(int id)
        {
            Order = await OrderService.GetOrderByIdAsync(id);
            User = await UserService.GetUserByHttpContextAsync(HttpContext);
            OrderLines = (await OrderService.GetOrderlinesByOrderIdAsync(id)).ToList();
            OrderSum = await OrderService.GetOrderSumAsync(OrderLines);
        }

       
    }
}
