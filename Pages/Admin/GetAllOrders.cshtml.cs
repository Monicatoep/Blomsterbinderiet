using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Enum;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin, Employee")]
    public class GetAllOrdersModel : PageModel
    {
        public OrderService OrderService { get; set; }
        public UserService UserService { get; set; }
        public IEnumerable<Models.Order> MyOrders { get; set; }


        public GetAllOrdersModel(OrderService orderService, UserService userService)
        {
            OrderService = orderService;
            UserService = userService;
        }
        public IActionResult OnGet()
        {
            MyOrders = OrderService.GetAllOrders();

            return Page();
        }
        public async Task<IActionResult> OnPostDenyAsync(int id)
        {
            
            Order order= OrderService.GetOrderById(id);
            order.OrderStatus = Status.Afvist;
            await OrderService.UpdateOrderAsync(order);
            MyOrders = OrderService.GetAllOrders();
            return Page();
        }
        public async Task<IActionResult> OnPostConfirmAsync(int id)
        {
            
            Order order = OrderService.GetOrderById(id);
            order.OrderStatus = Status.Bekræftet;
            await OrderService.UpdateOrderAsync(order);
            MyOrders = OrderService.GetAllOrders();
            return Page();
        }
        public async Task<IActionResult> OnPostInProgressAsync(int id)
        {

            Order order = OrderService.GetOrderById(id);
            order.OrderStatus = Status.Klargøres;

            string userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                if (userId != null)
                {
                    order.Employee = UserService.GetUserByIdAsync(Convert.ToInt32(userId));
                    order.EmployeeID = order.Employee.ID;
                }
            
            await OrderService.UpdateOrderAsync(order);
            MyOrders = OrderService.GetAllOrders();
            return Page();
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id)
        {
            Order order = OrderService.GetOrderById(id);
            if(order.OrderStatus == Status.Klargøres)
            {
                order.OrderStatus = Status.Færdig;
            }
            else if(order.OrderStatus == Status.Færdig)
            {
                order.OrderStatus = Status.Udleveret;
            }
            await OrderService.UpdateOrderAsync(order);
            MyOrders = OrderService.GetAllOrders();
            return Page(); 
        }
    }
}
