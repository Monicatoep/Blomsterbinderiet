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
            int userId = (await UserService.GetUserByHttpContext(HttpContext)).ID;
            Orders = from o in OrderService.GetAllOrders()
                     where o.CustomerID == userId
                     join l in OrderService.OrderlineService.GetObjectsAsync().Result on o.Id equals l.OrderID into ol
                     orderby o.OrderDate descending
                     select new MyOrders { Order = o, OrderLine = ol, Amount = ol.Sum((OrderLine o) => o.Amount) };
        }
    }
}
