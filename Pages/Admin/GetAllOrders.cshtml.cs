using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin, Employee")]
    public class GetAllOrdersModel : PageModel
    {
        public OrderService OrderService { get; set; }
        public IEnumerable<Models.Order> MyOrders { get; set; }


        public GetAllOrdersModel(OrderService orderService)
        {
            OrderService = orderService;
        }
        public IActionResult OnGet()
        {
            MyOrders = OrderService.GetAllOrders();

            return Page();
        }

        public async Task<IActionResult> OnPostDenyAsync(int id)
        {
            
            Order order= OrderService.GetOrderById(id);
            order.OrderStatus = Order.Status.Afvist;
            await OrderService.UpdateOrderAsync(order);
            MyOrders = OrderService.GetAllOrders();
            return Page();
        }
        public async Task<IActionResult> OnPostConfirmAsync(int id)
        {
            
            Order order = OrderService.GetOrderById(id);
            order.OrderStatus = Order.Status.Bekræftet;
            await OrderService.UpdateOrderAsync(order);
            MyOrders = OrderService.GetAllOrders();
            return Page();
        }
    }
}
