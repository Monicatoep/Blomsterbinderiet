using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.UserPages
{
    public class OrderDetailsModel : PageModel
    {
        public OrderService OrderService { get; set; }
        public UserService UserService { get; set; }
        public ProductService ProductService { get; set; }
        [BindProperty]
        public Order Order { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public User User { get; set; }
        public double OrderSum { get; set; }
        public string Message { get; set; }

        public OrderDetailsModel(OrderService orderService, UserService userService, ProductService productService)
        {
            OrderService = orderService;
            UserService = userService;
            ProductService = productService;
            OrderLines = new List<OrderLine>();
            
        }

        public async Task OnGetAsync(int id)
        {
            Task<Order> getOrder = OrderService.GetOrderByIdAsync(id);
            Task<User?> getUser = UserService.GetUserByHttpContextAsync(HttpContext);
            Task<IEnumerable<OrderLine>> getOrderLines = OrderService.GetOrderlinesByOrderIdAsync(id);

            OrderLines = (await getOrderLines).ToList();
            OrderSum = await OrderService.GetOrderSumAsync(OrderLines);
            Order = await getOrder;
            User = await getUser;
        }    
        
        public async Task<IActionResult> OnPostAsync(int id)
        {
            Order.Id = id;
            await OrderService.UpdateOrderAsync(Order, nameof(Order.CommentShop));

            Task<Order> getOrder = OrderService.GetOrderByIdAsync(id);
            Task<User?> getUser = UserService.GetUserByHttpContextAsync(HttpContext);
            Task<IEnumerable<OrderLine>> getOrderLines = OrderService.GetOrderlinesByOrderIdAsync(id);

            OrderLines = (await getOrderLines).ToList();
            OrderSum = await OrderService.GetOrderSumAsync(OrderLines);
            Order = await getOrder;
            User = await getUser;
            return Page();
        }
    }
}
