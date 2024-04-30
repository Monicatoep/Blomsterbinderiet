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
            MyOrders = OrderService.GetAllOrdersAsync().Result;

            return Page();
        }
        public async Task<IActionResult> OnPostDenyAsync(int id)
        {
            OrderService.DenyOrderAsync(id);
            MyOrders = await OrderService.GetAllOrdersAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostConfirmAsync(int id)
        {
            OrderService.ConfirmOrderAsync(id);
            MyOrders = await OrderService.GetAllOrdersAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostInProgressAsync(int id)
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            await OrderService.OrderInProgressAsync(id, userId);
            MyOrders = await OrderService.GetAllOrdersAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(int id)
        {
            OrderService.ChangeOrderStatusAsync(id);
            MyOrders = await OrderService.GetAllOrdersAsync();
            return Page(); 
        }
    }
}
