using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.DAO;

namespace Blomsterbinderiet.Pages.UserPages
{
    public class MyOrdersModel : PageModel
    {
        public UserService UserService { get; set; }
        public OrderService OrderService { get; set; }
        public IEnumerable<MyOrdersDAO> Orders { get; set; }

        public MyOrdersModel(UserService userService, OrderService orderService)
        {
            UserService = userService;
            OrderService = orderService;
        }

        public async Task OnGet()
        {
            Orders = await OrderService.GetOrdersByUserId((UserService.GetUserByHttpContext(HttpContext)).Result.ID);
        }
    }
}
