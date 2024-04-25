using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Pages.UserPages
{
    public class MyOrdersModel : PageModel
    {
        public UserService UserService { get; set; }
        public OrderService OrderService { get; set; }
        public IEnumerable<test> Orders { get; set; }

        public MyOrdersModel(UserService userService, OrderService orderService)
        {
            UserService = userService;
            OrderService = orderService;
        }

        public async Task OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                Orders = from p in OrderService.GetAllOrders() 
                         where p.CustomerID+"" == userId
                         join l in OrderService.OrderlineService.GetObjectsAsync().Result on p.Id equals l.OrderID into ps
                         orderby p.OrderDate
                         select new test { hello = p, hello2 = ps, Amount = ps.Sum((OrderLine o) => o.Amount)};
               
            }
        }
    }
    //https://stackoverflow.com/questions/12259365/create-anonymous-object-via-linq
    public class test
    {
        public Order hello;
        public IEnumerable<OrderLine> hello2;
        public int Amount;
    }
}
