using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.DAO;

namespace Blomsterbinderiet.Pages.UserPages
{
    public class MyOrdersModel : PageModel
    {
        private UserService UserService { get; set; }
        private OrderService OrderService { get; set; }
        public IEnumerable<MyOrdersDAO> Orders { get; set; }

        public MyOrdersModel(UserService userService, OrderService orderService)
        {
            UserService = userService;
            OrderService = orderService;
        }

        public async Task OnGetAsync()
        {
            Orders = await OrderService.GetOrdersByUserIdAsync((UserService.GetUserByHttpContextAsync(HttpContext)).Result.ID);
        }

        public void OnPostAsync()
        {

        }
    }
}
