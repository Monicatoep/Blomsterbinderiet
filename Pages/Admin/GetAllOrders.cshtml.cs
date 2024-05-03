using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blomsterbinderiet.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.EntityFrameworkCore.Storage.Internal;
using System.Security.Claims;
using System.Collections.Immutable;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin, Employee")]
    public class GetAllOrdersModel : PageModel
    {
        public OrderService OrderService { get; set; }
        public UserService UserService { get; set; }
        public List<Models.Order> MyOrders { get; set; }
        public List<User> Employees { get; set; }
        public Status[] StatusList{ get; set; }
        [BindProperty]
        public DateOnly Date { get; set; }
      
        public GetAllOrdersModel(OrderService orderService, UserService userService)
        {
            OrderService = orderService;
            UserService = userService;
            Employees = userService.GetEmployees().Result.ToList();
            StatusList = (Status[])Enum.GetValues(typeof(Status));
        }
        public async Task<IActionResult> OnGetAsync()
        {
            MyOrders =  await OrderService.GetAllOrdersWeekAsync();

            return Page();
        }
        public async Task<IActionResult> OnGetSeeAllAsync()
        {
            MyOrders = await OrderService.GetAllOrdersAsync();

            return Page();
        }
        public async Task<IActionResult> OnGetResetAsync()
        {
            MyOrders =  await OrderService.GetAllOrdersWeekAsync();

            return Page();
        }

        #region Due date filter and sort
        public IActionResult OnGetSortByDueDate()
        {
            MyOrders = OrderService.SortByDueDate().ToList();
            return Page();
        }
        public IActionResult OnGetSortByDueDateDes()
        {
            MyOrders = OrderService.SortByDueDateDes().ToList();
            return Page();
        }
        public IActionResult OnPostFilterByDueDate()
        {
            
            MyOrders = OrderService.FilterByDueDate(Date).ToList();
            return Page();
        }
        public IActionResult OnPostFilterByDueDateToday()
        {
            
            MyOrders = OrderService.FilterByDueDateToday().ToList();
            return Page();
        }
        #endregion

        #region Employee filter and sort
        public IActionResult OnGetSortByEmployee()
        {
            MyOrders = OrderService.SortByEmployee().ToList();
            return Page();
        }
        public IActionResult OnGetSortByEmployeeDes()
            {
            MyOrders = OrderService.SortByEmployeeDes().ToList();
            return Page();

        }
        public IActionResult OnGetFilterByEmployee(int id)
        {
            MyOrders = OrderService.FilterByEmployee(id).ToList();
            return Page();
        }
        public IActionResult OnGetFilterByEmployeeNull()
        {
            MyOrders = OrderService.FilterByEmployeeNull().ToList();
            return Page();
        }
        #endregion

        #region Status filter and sort
        public IActionResult OnGetSortByStatus()
        {
            MyOrders = OrderService.SortByStatus().ToList();
            return Page();
        }
        public IActionResult OnGetSortByStatusDes()
        {
            MyOrders = OrderService.SortByStatusDes().ToList();
            return Page();
        }
        public IActionResult OnGetFilterByStatus(Status status)
        {
            MyOrders = OrderService.FilterByStatus(status).ToList();
            return Page();
        }
        #endregion

        #region Status onpost change
        public async Task<IActionResult> OnPostDenyAsync(int id)
        {
            await OrderService.DenyOrderAsync(id);
            MyOrders = await OrderService.GetAllOrdersAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostConfirmAsync(int id)
        {
            await OrderService.ConfirmOrderAsync(id);
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
            await OrderService.ChangeOrderStatusAsync(id);
            MyOrders = await OrderService.GetAllOrdersAsync();
            return Page(); 
        }
        #endregion
    }
}
